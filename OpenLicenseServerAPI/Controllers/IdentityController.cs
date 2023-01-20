using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using OpenLicenseServerAPI.Utils;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.Facades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.Facades;

namespace OpenLicenseServerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController : ControllerBase
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IUserFacade _userFacade;
    private readonly IMapper _mapper;

    public IdentityController(SignInManager<IdentityUser> signInManager,
        ILogger<IdentityController> logger,
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        IUserFacade userFacade,
        IMapper mapper)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
        _configuration = configuration;
        _userFacade = userFacade;
        _mapper = mapper;
    }
    
    [Authorize]
    [HttpGet("Current")]
    public async Task<IActionResult> GetCurrentUserAsync()
    {
        var userId = ApiUtils.GetCurrentUserId(User);
        if (userId is not int currentUserId)
        {
            return BadRequest("Your session has expired, please logout and login again.");
        }

        var userDto = await _userFacade.GetUserByIdAsync(currentUserId);

        if (userDto == null) return NotFound($"User with id:{currentUserId} doesn't exist.");

        userDto.IsAdmin = User.IsAdmin();
        
        return Ok(userDto);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserCreateDto userCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var newUser = new IdentityUser
        {
            Email = userCreateDto.Email,
            UserName = userCreateDto.FirstName + userCreateDto.LastName
        };

        var existingUser = await _userManager.FindByEmailAsync(newUser.Email).ConfigureAwait(false);
        if (existingUser != null)
        {
            return BadRequest("Provided email address is already used in the system.");
        }

        var createResult = await _userManager.CreateAsync(newUser, userCreateDto.Password).ConfigureAwait(false);

        if (!createResult.Succeeded)
        {
            return BadRequest(createResult);
        }

        await _userFacade.CreateUserAsync(userCreateDto);
        var user = await _userManager.FindByEmailAsync(newUser.Email).ConfigureAwait(false);

        _logger.LogInformation($"Successfuly created a new user account: {user.Email}");

        await _userManager.AddToRoleAsync(user, "User").ConfigureAwait(false);

        await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

        var userDto = await _userFacade.GetUserByEmailAsync(user.Email);

        var userWithTokenDto = _mapper.Map<UserWithTokenDto>(userDto);
        userWithTokenDto.Token = await GenerateJwtToken(user, userDto);
        return Ok(userWithTokenDto);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email).ConfigureAwait(false);

        if (user is null || await _userManager.CheckPasswordAsync(user, userLoginDto.Password).ConfigureAwait(false) ==
            false)
        {
            return BadRequest("Email or password is incorrect.");
        }

        await _signInManager.SignOutAsync().ConfigureAwait(false);
        var loggedIn = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, true, false)
            .ConfigureAwait(false);

        if (!loggedIn.Succeeded || await _userFacade.GetUserByEmailAsync(user.Email) is not UserDto userDto)
        {
            _logger.LogWarning($"Error logging in user {userLoginDto.Email}.");
            return BadRequest("Unexpected problem with login, contact Foodlivery support.");
        }
        
        var userWithTokenDto = _mapper.Map<UserWithTokenDto>(userDto);
        userWithTokenDto.Token = await GenerateJwtToken(user, userDto);
        userWithTokenDto.IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        return Ok(userWithTokenDto);
    }

    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        await HttpContext.SignOutAsync();
        return Ok();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{email}/UpdateAdminRole")]
    public async Task<IActionResult> AddAdminRoleToUser([EmailAddress] string email)
    {
        var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

        if (user is null)
        {
            return BadRequest("User with given email does not exist.");
        }

        var isAdmin = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        if (isAdmin.Contains("Admin"))
        {
            var removed = await _userManager.RemoveFromRoleAsync(user, "Admin").ConfigureAwait(false);

            if (!removed.Succeeded)
            {
                return BadRequest($"Admin role could not be removed for user: {email}");
            }

            return Ok($"Admin role has been removed for user: {email}");
        }

        var added = await _userManager.AddToRoleAsync(user, "Admin").ConfigureAwait(false);

        if (!added.Succeeded)
        {
            return BadRequest($"Admin role could not be added to user: {email}");
        }

        return Ok($"Admin role has been added to user: {email}");
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("Users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userFacade.GetAllUsersAsync();
        foreach (var user in users)
        {
            var userIdentity = await _userManager.FindByEmailAsync(user.Email);
            user.IsAdmin = await _userManager.IsInRoleAsync(userIdentity, "Admin");
        }
        return Ok(users);
    }

    [Authorize]
    [HttpPut("Profile")]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto userUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = ApiUtils.GetCurrentUserId(User);
        if (userId == null)
        {
            return BadRequest("Your session has expired, please logout and login again.");
        }

        if (userId != userUpdateDto.Id.Value)
        {
            return BadRequest("It is permitted to update only own profile.");
        }

        await _userFacade.UpdateUserAsync(userUpdateDto);

        var userDto = await _userFacade.GetUserByIdAsync(userUpdateDto.Id.Value);
        userDto.IsAdmin = User.IsAdmin();

        return Ok(userDto);
    }

    [Authorize]
    [HttpGet("Profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = ApiUtils.GetCurrentUserId(User);
        if (userId is not int currentUserId)
        {
            return BadRequest("Your session has expired, please logout and login again.");
        }

        var userDto = await _userFacade.GetUserByIdAsync(currentUserId);

        if (userDto == null) return NotFound($"User with id:{currentUserId} doesn't exist.");
        
        userDto.IsAdmin = User.IsAdmin();

        return Ok(userDto);
    }

    [Authorize]
    [HttpPut("Profile/Password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = ApiUtils.GetCurrentUserId(User);
        if (userId == null)
        {
            return BadRequest("Your session has expired, please logout and login again.");
        }

        if (userId != changePasswordDto.UserId)
        {
            return BadRequest("It is permitted to update only own password.");
        }

        var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
        
        if(!await _userManager.CheckPasswordAsync(user, changePasswordDto.OldPassword))
        {
            return BadRequest("Old password is not correct.");
        }
        
        var changed =
            await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!changed.Succeeded)
        {
            return BadRequest(changed);
        }

        return Ok();
    }

    private async Task<string> GenerateJwtToken(IdentityUser user, UserDto userDto)
    {
        var credentials = GetSigningCredentials();
        var claims = GetClaims(user, userDto);
        var tokenOptions = GenerateTokenOptions(credentials, await claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: signingCredentials);
        return tokenOptions;
    }

    private async Task<List<Claim>> GetClaims(IdentityUser user, UserDto userDto)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ApiUtils.UserIdClaimType, userDto.Id.ToString())
        };
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }
}