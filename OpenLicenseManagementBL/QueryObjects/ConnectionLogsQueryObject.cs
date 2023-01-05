using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class ConnectionLogsQueryObject: QueryObject<ConnectionLog>
{
    public ConnectionLogsQueryObject(IQuery<ConnectionLog> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public Task<QueryResult<ConnectionLog>> ExecuteQueryAsync(ConnectionLogFilterDto filterDto)
    {
        var query = Query.Where<int>(a => a == filterDto.DeviceId, nameof(ConnectionLog.DeviceId));

        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
