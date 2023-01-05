using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Services;

public interface IConnectionLogService
{
    Task CreateAsync(ConnectionLog log);

    Task<ConnectionLog?> GetById(int id);

    Task UpdateAsync(ConnectionLog license);

    Task DeleteAsync(int id);

    Task<IEnumerable<ConnectionLog>> GetAllAsync();
}