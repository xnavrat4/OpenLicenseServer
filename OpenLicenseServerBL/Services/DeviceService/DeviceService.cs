using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Infrastructure.EFCore.Query;
using Infrastructure.Repository;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerDAL.Models;
using static System.Text.Json.JsonSerializer;

namespace OpenLicenseServerBL.Services;

public class DeviceService : IDeviceService
{
    private readonly IRepository<Device> _deviceRepository;
    private readonly DeviceQueryObject _queryObject;
    private readonly DeviceHWInfoHashQueryObject _deviceHwInfoHashQueryObject;
    private readonly DeviceValidationQueryObject _deviceValidationQueryObject;

    public DeviceService(IRepository<Device> deviceRepository, DeviceQueryObject queryObject, DeviceHWInfoHashQueryObject deviceHwInfoHashQueryObject, DeviceValidationQueryObject deviceValidationQueryObject)
    {
        _deviceRepository = deviceRepository;
        _queryObject = queryObject;
        _deviceHwInfoHashQueryObject = deviceHwInfoHashQueryObject;
        _deviceValidationQueryObject = deviceValidationQueryObject;
    }

    public async Task CreateAsync(Device device)
    {
        await _deviceRepository.Insert(device);
    }

    public async Task RegisterAsync(Device device)
    {
        await _deviceRepository.Insert(device);
    }

    public async Task<Device?> GetById(int id)
    {
        return await _deviceRepository.GetById(id, new []{nameof(Device.Licenses), nameof(Device.Violations), nameof(Device.Customer), nameof(Device.HwInfo)});
    }

    public async Task UpdateAsync(Device device)
    {
        _deviceRepository.Update(device);
        await _deviceRepository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _deviceRepository.Delete(id);
    }

    public async Task<IEnumerable<Device>> GetAllAsync()
    {
        return await _deviceRepository.Get();
    }

    public Task<QueryResult<Device>> GetFilteredDevices(FilterDto filterDto)
    {
        return _queryObject.ExecuteQueryAsync(filterDto);
    }

    public Task<QueryResult<Device>> GetDevicesWithValidity(DeviceValidityDto filterDto)
    {
        return _deviceValidationQueryObject.ExecuteQueryAsync(filterDto);
    }
}
