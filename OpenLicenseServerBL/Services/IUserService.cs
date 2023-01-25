using Infrastructure.EFCore.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IUserService
{
    Task CreateAsync(User user);

    Task<User?> GetById(int id);
    Task<QueryResult<User>> GetUserByEmailAsync(string email);

    Task UpdateAsync(User user);

    Task DeleteAsync(int id);

    Task<IEnumerable<User>> GetAllAsync();
}