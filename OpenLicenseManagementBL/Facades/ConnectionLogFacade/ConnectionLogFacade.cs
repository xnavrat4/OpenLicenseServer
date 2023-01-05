using AutoMapper;
using Infrastructure.UnitOfWork;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.QueryObjects;
using OpenLicenseManagementBL.Services;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Facades;

public class ConnectionLogFacade : IConnectionLogFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConnectionLogService _connectionLogService;
    private readonly IViolationService _violationService;
    private DeviceHWInfoHashQueryObject _deviceHwInfoHashQueryObject;
    private readonly ConnectionLogsQueryObject _connectionLogsQueryObject;
    private readonly ViolationsOfDeviceQueryObject _violationsOfDeviceQueryObject;

    public ConnectionLogFacade(IMapper mapper, IUnitOfWork unitOfWork, IConnectionLogService connectionLogService, DeviceHWInfoHashQueryObject deviceHwInfoHashQueryObject, ConnectionLogsQueryObject connectionLogsQueryObject, IViolationService violationService, ViolationsOfDeviceQueryObject violationsOfDeviceQueryObject)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _connectionLogService = connectionLogService;
        _deviceHwInfoHashQueryObject = deviceHwInfoHashQueryObject;
        _connectionLogsQueryObject = connectionLogsQueryObject;
        _violationService = violationService;
        _violationsOfDeviceQueryObject = violationsOfDeviceQueryObject;
    }

    public async Task<ConnectionLogDto?> GetLogByIdAsync(int logId)
    {
        var log = await _connectionLogService.GetById(logId);
        return _mapper.Map<ConnectionLogDto>(log);
    }

    public async Task UpdateLogAsync(ConnectionLogDto logDto)
    {
        var log = _mapper.Map<ConnectionLog>(logDto);
        await _connectionLogService.UpdateAsync(log);
    }

    public async Task<IEnumerable<ConnectionLogDto>> GetAllLogsAsync()
    {
        var logs = await _connectionLogService.GetAllAsync();
        return _mapper.Map<IEnumerable<ConnectionLogDto>>(logs);
    }

    public async Task<QueryResultDto<ConnectionLogDto>> GetAllFilteredLogsAsync(ConnectionLogFilterDto filterDto)
    {
        var logs = await _connectionLogsQueryObject.ExecuteQueryAsync(filterDto);
        return _mapper.Map<QueryResultDto<ConnectionLogDto>>(logs);
    }

    public async Task<QueryResultDto<ViolationDto>> GetViolationsOfDevice(int deviceId)
    {
        var violations = await _violationsOfDeviceQueryObject.ExecuteQueryAsync(deviceId);
        return _mapper.Map<QueryResultDto<ViolationDto>>(violations);
    }

    public async Task DeleteLogAsync(int logId)
    {
        await _connectionLogService.DeleteAsync(logId);
    }
}
