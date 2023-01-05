using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenLicenseManagementAPI.Utils;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.DTOs.User;
using OpenLicenseManagementBL.Enums;
using OpenLicenseManagementBL.Facades;

namespace OpenLicenseManagementAPI.Controllers;

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
    
    /// <summary>
    /// Allows user to authenticate themselves with a JWT  
    /// </summary>
    /// <returns>UserDTO of a authenticated user or a BadRequest</returns>
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

        if (userDto == null)
        {
            return NotFound($"User with id:{currentUserId} doesn't exist.");
        }

        if ((int)userDto.UserStatus == (int)UserStatus.NotValidated)
        {
            return BadRequest($"User with id:{userDto.Id} is not validated by administrators yet");
        }

        userDto.IsAdmin = User.IsAdmin();
        
        return Ok(userDto);
    }
/// <summary>
/// Registers a new user
/// </summary>
/// <param name="userCreateDto">A DTO representing the new user</param>
/// <returns>OK if the email address is not taken</returns>
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

        return Ok();
    }

/// <summary>
/// Logs in a user.
/// </summary>
/// <param name="userLoginDto">A user email with a password</param>
/// <returns>A user with token if the login is successful otherwise return a BadRequest</returns>
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
            return BadRequest("Unexpected problem with login");
        }

        if ((int)userDto.UserStatus != (int)UserStatus.Validated)
        {
            return BadRequest("User is not yet validated by admins");
        }
        
        var userWithTokenDto = _mapper.Map<UserWithTokenDto>(userDto);
        userWithTokenDto.Token = await GenerateJwtToken(user, userDto);
        userWithTokenDto.IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

        return Ok(userWithTokenDto);
    }
/// <summary>
/// Logs out a user.
/// </summary>
/// <returns>OK</returns>
    [Authorize]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync().ConfigureAwait(false);
        await HttpContext.SignOutAsync();
        return Ok();
    }
    
/// <summary>
/// Promotes a user to an admin role or demotes them back to standard user
/// </summary>
/// <param name="email">Email of a user to be promoted or demoted</param>
/// <returns>OK</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("UpdateAdminRole/{email}")]
    public async Task<IActionResult> AddAdminRoleToUser([FromRoute] string email)
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

        _logger.LogInformation($"Admin role has been added to user: {email}");
        return Ok($"Admin role has been added to user: {email}");
    }

/// <summary>
/// Gets all users.
/// </summary>
/// <returns>All users</returns>
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

/// <summary>
/// Updates user information such as First name, Last name and phone number.
/// </summary>
/// <param name="userUpdateDto">A user object to be updated</param>
/// <returns>OK</returns>
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

/// <summary>
/// Updates user password.
/// </summary>
/// <param name="changePasswordDto">Old and new passwords</param>
/// <returns>OK</returns>
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

/// <summary>
/// Gets user profile data
/// </summary>
/// <returns>User profile data</returns>
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
        
        if (userDto == null)
        {
            return NotFound($"User with id:{currentUserId} doesn't exist.");
        }
        
        if ((int)userDto.UserStatus == (int)UserStatus.NotValidated)
        {
            return BadRequest($"User with id:{userDto.Id} is not validated by administrators yet");
        }
        
        userDto.IsAdmin = User.IsAdmin();

        return Ok(userDto);
    }

/// <summary>
/// Validate user - acknowledge them and allow them the access to the system.
/// </summary>
/// <param name="validateDto">User that is to be validated</param>
/// <returns>Ok</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("Validate")]
    public async Task<IActionResult> ValidateUser(UserValidateDto validateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _userFacade.ValidateUserAsync(validateDto);

        return Ok();
    }
  
/// <summary>
/// Gets validated or non validated users.
/// </summary>
/// <param name="filterDto">Filter parameters</param>
/// <returns>All User Dtos that conform to the parameters</returns>
    [Authorize(Roles = "Admin")]  
    [HttpGet("Validated")]
    public async Task<IActionResult> GetValidatedUsers([FromQuery] UserStatusFilterDto filterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var users = await _userFacade.GetUsersOfStatus(filterDto);
        foreach (var user in users.Items)
        {
            var userIdentity = await _userManager.FindByEmailAsync(user.Email);
            user.IsAdmin = user.IsAdmin = await _userManager.IsInRoleAsync(userIdentity, "Admin");
        }
        
        return Ok(users);
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
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }
}