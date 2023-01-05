using AutoMapper;
using Infrastructure.EFCore.Query;
using Infrastructure.UnitOfWork;
using OpenLicenseBL.DTOs;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerBL.DTOs.HWInfo;
using OpenLicenseServerBL.DTOs.HWInfo.OSDtos;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerBL.Services;
using OpenLicenseServerDAL.Models;
using OperatingSystem = OpenLicenseServerDAL.Models.HWIdentifiers.OperatingSystem;

namespace OpenLicenseServerBL.Facades;

public class DeviceFacade : IDeviceFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeviceService _deviceService;
    private readonly IViolationService _violationService;
    private DeviceHWInfoHashQueryObject _deviceHwInfoHashQueryObject;

    public DeviceFacade(IMapper mapper, IUnitOfWork unitOfWork, IDeviceService deviceService, DeviceHWInfoHashQueryObject deviceHwInfoHashQueryObject, IViolationService violationService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _deviceService = deviceService;
        _deviceHwInfoHashQueryObject = deviceHwInfoHashQueryObject;
        _violationService = violationService;
    }


    public async Task<Tuple<Device,string>> RegisterDeviceAsync(DeviceRegisterDto deviceToRegister, string hwInfoHash)
    {
        var device = _mapper.Map<Device>(deviceToRegister);
        device.HWInfoHash = hwInfoHash;
        device.Validated = false;
        device.LastOnline = DateTime.UtcNow;
        await _deviceService.CreateAsync(device);
        await _unitOfWork.CommitAsync();
        var dbDevice = await _deviceHwInfoHashQueryObject.ExecuteQueryAsync(new DeviceHWInfoHashFilterDto()
            { HWInfoHash = hwInfoHash });
        return new Tuple<Device, string>(dbDevice.Items.First(), hwInfoHash);
    }

    public async Task ValidateDeviceAsync(int deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        device.Validated = true;
        await _deviceService.UpdateAsync(device);
        await _unitOfWork.CommitAsync();
    }

    public Task<Device?> GetById(int deviceId)
    {
        return _deviceService.GetById(deviceId);
    }

    public async Task<string> GetDevicePublicKey(int deviceId)
    {
        var device = await _deviceService.GetById(deviceId);
        var pk = device.PublicKey;
        return pk;
    }

    public async Task DeviceConnectedAsync(Device device)
    {
        device.LastOnline = DateTime.UtcNow;
        await _deviceService.UpdateAsync(device);
        await _unitOfWork.CommitAsync();
    }

    public async Task ReportViolationsAsync(DeviceViolationsDto violationsDto)
    {
        foreach (var violation in violationsDto.Violations.Select(violationDto => _mapper.Map<Violation>(violationDto)))
        {
            await _violationService.CreateAsync(violation);
        }
    }

    public DeviceReportDto ConvertToDeviceReportDto(Device device)
    {
        return _mapper.Map<DeviceReportDto>(device);
    }

    public async Task<Tuple<Device?, string>> FindDeviceByHWInfoHash(HWInfoCreateDto hwInfoCreateDto)
    {
        var hash = DeviceHasher.Hash(hwInfoCreateDto);

        var qr = await _deviceHwInfoHashQueryObject.ExecuteQueryAsync(new DeviceHWInfoHashFilterDto() { HWInfoHash = hash });
        return new Tuple<Device?, string>(qr.Items.FirstOrDefault(), hash);
    }

    public async Task<DeviceReportDto> ReportDevice(ReportRequestDto reportRequestDto)
    {
        var device = await _deviceService.GetById(reportRequestDto.DeviceId);
        if (reportRequestDto.Violations.Any())
        {
            foreach (var violationDto in reportRequestDto.Violations)
            {
                var violation = _mapper.Map<Violation>(violationDto);
                violation.DeviceId = reportRequestDto.DeviceId;
                await _violationService.CreateAsync(violation);
                await _unitOfWork.CommitAsync();
            }
        }
        await DeviceConnectedAsync(device);
        var newDev = await GetById(reportRequestDto.DeviceId);
        return _mapper.Map<DeviceReportDto>(newDev);
    }
}
