using AutoMapper;
using Infrastructure.EFCore.Query;
using Infrastructure.UnitOfWork;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Device;
using OpenLicenseManagementBL.DTOs.HWInfo;
using OpenLicenseManagementBL.DTOs.HWInfo.OSDtos;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.QueryObjects;
using OpenLicenseManagementBL.Services;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerDAL.Models;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseManagementBL.Facades;

public class DeviceFacade : IDeviceFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeviceService _deviceService;
    private readonly IViolationService _violationService;
    private DeviceHWInfoHashQueryObject _deviceHwInfoHashQueryObject;
    private readonly DeviceViolationsQueryObject _deviceViolationsQueryObject;

    public DeviceFacade(IMapper mapper, IUnitOfWork unitOfWork, IDeviceService deviceService, DeviceHWInfoHashQueryObject deviceHwInfoHashQueryObject, IViolationService violationService, DeviceViolationsQueryObject deviceViolationsQueryObject)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _deviceService = deviceService;
        _deviceHwInfoHashQueryObject = deviceHwInfoHashQueryObject;
        _violationService = violationService;
        _deviceViolationsQueryObject = deviceViolationsQueryObject;
    }

    public async Task CreateDeviceAsync(DeviceValidateDto deviceValidateDto)
    {
        var device = _mapper.Map<Device>(deviceValidateDto);
        await _deviceService.CreateAsync(device);
        await _unitOfWork.CommitAsync();
    }

    public async Task<DeviceDetailDto?> GetDeviceByIdAsync(int deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        return _mapper.Map<DeviceDetailDto>(device);
    }

    public async Task ValidateDeviceAsync(DeviceValidateDto deviceDto)
    {
        var device = _mapper.Map<Device>(deviceDto);
        var dev = await _deviceService.GetById(device.Id);
        dev.CustomerId = device.Customer?.Id;
        dev.Activated = device.Activated;
        dev.HeartbeatFrequency = deviceDto.HeartbeatFrequency;
        dev.SerialNumber = device.SerialNumber;
        dev.AdditionalInfo = device.AdditionalInfo;
        await _deviceService.UpdateAsync(dev);
    }

    public async Task ResolveViolationAsync(ViolationResolveDto resolveDto)
    {
        var violation = await _violationService.GetById(resolveDto.ViolationId);
        violation.Resolved = true;
        await _violationService.UpdateAsync(violation);
    }

    public async Task UpdateDeviceAsync(DeviceDto deviceDto)
    {
        var device = _mapper.Map<Device>(deviceDto);
        await _deviceService.UpdateAsync(device);
    }

    public async Task<HWInfoDto> GetDeviceHWInfo(int deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        return _mapper.Map<HWInfoDto>(device?.HwInfo);
    }

    public async Task<IEnumerable<DeviceDto>> GetAllDeviceAsync()
    {
        var  devices= await _deviceService.GetAllAsync();
        return _mapper.Map<IEnumerable<DeviceDto>>(devices);
    }

    public async Task<IEnumerable<DeviceNameDto>> GetAllDeviceSimpleAsync()
    {
        var devices = await _deviceService.GetAllAsync();
        return _mapper.Map<IEnumerable<DeviceNameDto>>(devices);
    }

    public async Task DeleteDeviceAsync(int deviceId)
    {
        await _deviceService.DeleteAsync(deviceId);
    }

    public async Task<QueryResult<DeviceDto>> GetFilteredDevices(FilterDto filterDto)
    {
        var queryResult = await _deviceService.GetFilteredDevices(filterDto);
        return _mapper.Map<QueryResult<DeviceDto>>(queryResult);
    }

    public async Task<QueryResult<DeviceDto>> GetDevicesWithValidity(DeviceValidityDto filterDto)
    {
        var queryResult = await _deviceService.GetDevicesWithValidity(filterDto);
        return _mapper.Map<QueryResult<DeviceDto>>(queryResult);
    }

    public async Task<IEnumerable<LicenseDto>> GetDevicesLicenses(int deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        return _mapper.Map<IEnumerable<LicenseDto>>(device?.Licenses);
    }

    public async Task<IEnumerable<ViolationDto>> GetDevicesViolations(int deviceId)
    {
        var filterDto = new DeviceViolationsFilterDto()
            { DeviceId = deviceId, SortCriteria = "Id", SortDescending = true };
        var qr = await _deviceViolationsQueryObject.ExecuteQueryAsync(filterDto);
        return _mapper.Map<IEnumerable<ViolationDto>>(qr.Items);
    }
}
