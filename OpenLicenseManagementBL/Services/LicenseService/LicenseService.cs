using Infrastructure.Repository;
using License = OpenLicenseServerDAL.Models.License;

namespace OpenLicenseManagementBL.Services;

public class LicenseService : ILicenseService
{
    private readonly IRepository<License> _licenseRepository;
    
    public LicenseService(IRepository<License> licenseRepository)
    {
        _licenseRepository = licenseRepository;
    }


    public async Task CreateAsync(License license)
    {
        await _licenseRepository.Insert(license);
        await _licenseRepository.Save();
    }

    public async Task<License?> GetById(int id)
    {
        return await _licenseRepository.GetById(id, new []{nameof(License.Device)});
    }

    public async Task UpdateAsync(License license)
    {
        _licenseRepository.Update(license);
        await _licenseRepository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _licenseRepository.Delete(id);
        await _licenseRepository.Save();
    }

    public async Task<IEnumerable<License>> GetAllAsync()
    {
        return await _licenseRepository.Get();
    }
}