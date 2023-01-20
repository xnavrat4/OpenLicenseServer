using OpenLicenseServerBL.DTOs;

namespace OpenLicenseServerBL.Facades;

public interface IUserFacade
{
    Task CreateUserAsync(UserCreateDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);

    Task UpdateUserAsync(UserUpdateDto userDto);
    
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task DeleteUserAsync(int userId);
}