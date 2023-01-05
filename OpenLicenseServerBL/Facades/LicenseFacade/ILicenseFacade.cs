using Infrastructure.EFCore.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public interface ILicenseFacade
{
    Task<LicenseDto> GetValidateLicense(LicenseValidateDto validateDto);
    Task<IEnumerable<License>> GetLicenseByLicenseKey(string serialNumber);

    Task<LicenseDto> AssignLicenseToDevice(License license, int deviceId);
    
    LicenseDto ReturnLicenseToDevice(License license);
    
}