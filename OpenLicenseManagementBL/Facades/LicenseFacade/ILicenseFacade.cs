using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerBL.DTOs.Device;

namespace OpenLicenseManagementBL.Facades;

public interface ILicenseFacade
{
    Task<LicenseDto> CreateLicenseAsync(LicenseCreateDto licenseDto);
    
    Task<LicenseDetailDto?> GetLicenseByIdAsync(int licenseId);

    Task UpdateLicenseAsync(LicenseDto licenseDto);
    
    Task<IEnumerable<LicenseDto>> GetAllLicensesAsync();
    
    Task DeleteLicenseAsync(int licenseId);
    Task<QueryResult<LicenseDto>> GetFilteredLicenses(FilterDto filterDto);

}