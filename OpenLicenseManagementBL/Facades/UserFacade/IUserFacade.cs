using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.DTOs.User;

namespace OpenLicenseManagementBL.Facades;

public interface IUserFacade
{
    Task CreateUserAsync(UserCreateDto userDto);
    Task<UserDto?> GetUserByIdAsync(int userId);
    Task<UserDto?> GetUserByEmailAsync(string email);

    Task UpdateUserAsync(UserUpdateDto userDto);

    Task ValidateUserAsync(UserValidateDto validateDto);

    Task<QueryResultDto<UserFullDto>> GetUsersOfStatus(UserStatusFilterDto filterDto);
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task DeleteUserAsync(int userId);
}