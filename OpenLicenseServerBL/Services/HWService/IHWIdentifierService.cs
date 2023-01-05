using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IHWIdentifierService
{
    Task CreateAsync(HWInfo hwInfo);

    Task<HWInfo?> GetById(int id);

    Task UpdateAsync(HWInfo hwInfo);

    Task DeleteAsync(int id);

    Task<IEnumerable<HWInfo>> GetAllAsync();
}