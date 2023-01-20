using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.Services;
using Infrastructure.UnitOfWork;
using OpenLicenseServerDAL.Models;
using AutoMapper;

namespace OpenLicenseServerBL.Facades;

public class UserFacade : IUserFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IService<User> _userService;

    public UserFacade(IUnitOfWork unitOfWork, IService<User> userService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _mapper = mapper;
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

    public Task<UserDto?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUserAsync(UserUpdateDto userDto)
    {
        var user = _mapper.Map<User>(userDto);
        await _userService.UpdateAsync(user);
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