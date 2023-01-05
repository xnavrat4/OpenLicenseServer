using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IViolationService
{
    Task CreateAsync(Violation violation);

    Task<Violation?> GetById(int id);

    Task UpdateAsync(Violation violation);

    Task DeleteAsync(int id);

    Task<IEnumerable<Violation>> GetAllAsync();
}