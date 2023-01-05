using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Facades;

public interface IConnectionLogFacade
{
    
    Task<ConnectionLogDto?> GetLogByIdAsync(int logId);
    
    Task UpdateLogAsync(ConnectionLogDto logDto);
    
    Task<IEnumerable<ConnectionLogDto>> GetAllLogsAsync();
    Task<QueryResultDto<ConnectionLogDto>> GetAllFilteredLogsAsync(ConnectionLogFilterDto filterDto);

    Task<QueryResultDto<ViolationDto>> GetViolationsOfDevice(int deviceId);

    Task DeleteLogAsync(int logId);
}