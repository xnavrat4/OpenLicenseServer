using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class DeviceViolationsQueryObject : QueryObject<Violation>
{
    public DeviceViolationsQueryObject(IQuery<Violation> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public Task<QueryResult<Violation>> ExecuteQueryAsync(DeviceViolationsFilterDto filterDto)
    {
        var query = Query.Where<int>(a => a == filterDto.DeviceId, nameof(Violation.DeviceId));
        
        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
