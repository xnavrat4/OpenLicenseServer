using Infrastructure.EFCore.Query;
using OpenLicenseBL.DTOs;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerBL.DTOs.HWInfo;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public interface IDeviceFacade
{

    Task<Tuple<Device,string>>  RegisterDeviceAsync(DeviceRegisterDto deviceToRegister, string hwInfoHash);
    Task ValidateDeviceAsync(int deviceId);

    Task<Device?> GetById(int deviceId);

    Task<string> GetDevicePublicKey(int deviceId);

    Task DeviceConnectedAsync(Device device);

    Task ReportViolationsAsync(DeviceViolationsDto violationsDto);
    DeviceReportDto ConvertToDeviceReportDto(Device device);

    Task<Tuple<Device?, string>> FindDeviceByHWInfoHash(HWInfoCreateDto hwInfoCreateDto);

    Task<DeviceReportDto> ReportDevice(ReportRequestDto requestDto);

}