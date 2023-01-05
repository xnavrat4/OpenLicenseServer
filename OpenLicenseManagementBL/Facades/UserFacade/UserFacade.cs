using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.Services;
using Infrastructure.UnitOfWork;
using OpenLicenseServerDAL.Models;
using AutoMapper;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.DTOs.User;
using OpenLicenseManagementBL.QueryObjects;

namespace OpenLicenseManagementBL.Facades;

public class UserFacade : IUserFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly UserStatusQueryObject _statusQueryObject;

    public UserFacade(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper, UserStatusQueryObject statusQueryObject)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _mapper = mapper;
        _statusQueryObject = statusQueryObject;
    }
    
    public async Task CreateUserAsync(UserCreateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userService.CreateAsync(user);
        await _unitOfWork.CommitAsync();
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        var user = await _userService.GetById(userId);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var queryResult = await _userService.GetUserByEmailAsync(email);
        return _mapper.Map<UserDto>(queryResult.Items.FirstOrDefault());
    }

    public async Task UpdateUserAsync(UserUpdateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        var userDb = await _userService.GetById(user.Id);
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.PhoneNumber = user.PhoneNumber;
        await _userService.UpdateAsync(userDb);
    }

    public async Task ValidateUserAsync(UserValidateDto validateDto)
    {
        var user = await _userService.GetById(validateDto.Id);
        user.UserStatus = (int)validateDto.UserStatus;
        user.AddedById = validateDto.AddedById;
        user.AddedOn = DateTime.Now.ToUniversalTime();
        await _userService.UpdateAsync(user);
    }

    public async Task<QueryResultDto<UserFullDto>> GetUsersOfStatus(UserStatusFilterDto filterDto)
    {
        var user = await _statusQueryObject.ExecuteQueryAsync(filterDto);
        return _mapper.Map<QueryResultDto<UserFullDto>>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllAsync(); 
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _userService.DeleteAsync(userId);
    }
}
