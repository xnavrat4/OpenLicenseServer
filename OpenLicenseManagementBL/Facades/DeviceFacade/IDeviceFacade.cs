using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Device;
using OpenLicenseManagementBL.DTOs.HWInfo;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Facades;

public interface IDeviceFacade
{
    Task CreateDeviceAsync(DeviceValidateDto deviceValidateDto);

    Task<DeviceDetailDto?> GetDeviceByIdAsync(int deviceDto);
    
    Task ValidateDeviceAsync(DeviceValidateDto deviceDto);

    Task ResolveViolationAsync(ViolationResolveDto resolveDto);
    Task UpdateDeviceAsync(DeviceDto deviceDto);
    Task<HWInfoDto> GetDeviceHWInfo(int deviceId);
    
    Task<IEnumerable<DeviceDto>> GetAllDeviceAsync();
    Task<IEnumerable<DeviceNameDto>> GetAllDeviceSimpleAsync();
    
    Task DeleteDeviceAsync(int deviceId);
    
    Task<QueryResult<DeviceDto>> GetFilteredDevices(FilterDto filterDto);
    Task<QueryResult<DeviceDto>> GetDevicesWithValidity(DeviceValidityDto filterDto);    
    
    Task<IEnumerable<LicenseDto>> GetDevicesLicenses(int deviceId);
    
    Task<IEnumerable<ViolationDto>> GetDevicesViolations(int deviceId);

}