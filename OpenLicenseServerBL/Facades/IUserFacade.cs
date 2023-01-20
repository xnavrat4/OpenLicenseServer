using OpenLicenseServerBL.DTOs;

namespace OpenLicenseServerBL.Facades;

public interface IUserFacade
{
    Task CreateUserAsync(UserCreateDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto?> GetUserByEmailAsync(string email);

    Task UpdateUserAsync(UserUpdateDto userDto);
    
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task DeleteUserAsync(int userId);
}