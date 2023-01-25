using OpenLicenseServerBL.DTOs.License;

namespace OpenLicenseServerBL.Facades;

public interface ILicenseFacade
{
    Task CreateLicenseAsync(LicenseCreateDto licenseDto);
    
    Task<LicenseDto?> GetLicenseByIdAsync(int licenseId);
    
    Task UpdateLicenseAsync(LicenseDto licenseDto);
    
    Task<IEnumerable<LicenseDto>> GetAllLicensesAsync();
    
    Task DeleteLicenseAsync(int licenseId);
}