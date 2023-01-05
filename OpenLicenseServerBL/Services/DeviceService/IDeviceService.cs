using Infrastructure.EFCore.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IDeviceService
{
    Task CreateAsync(Device device);
    
    Task RegisterAsync(Device device);

    Task<Device?> GetById(int id);

    Task UpdateAsync(Device device);

    Task DeleteAsync(int id);

    Task<IEnumerable<Device>> GetAllAsync();
    Task<QueryResult<Device>> GetFilteredDevices(FilterDto filterDto);

    Task<QueryResult<Device>> GetDevicesWithValidity(DeviceValidityDto filterDto);
}