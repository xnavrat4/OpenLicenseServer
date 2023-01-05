using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface ILicenseService
{
    Task CreateAsync(License license);

    Task<License?> GetById(int id);

    Task UpdateAsync(License license);

    Task DeleteAsync(int id);

    Task<IEnumerable<License>> GetAllAsync();
}