using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Services;

public interface IDeviceService
{
    Task CreateAsync(Device device);

    Task<Device?> GetById(int id);

    Task UpdateAsync(Device device);

    Task DeleteAsync(int id);

    Task<IEnumerable<Device>> GetAllAsync();
    Task<QueryResult<Device>> GetFilteredDevices(FilterDto filterDto);

    Task<QueryResult<Device>> GetDevicesWithValidity(DeviceValidityDto filterDto);
}