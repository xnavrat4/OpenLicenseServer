using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class ViolationsOfDeviceQueryObject : QueryObject<Violation>
{
    public ViolationsOfDeviceQueryObject(IQuery<Violation> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public Task<QueryResult<Violation>> ExecuteQueryAsync(int deviceId)
    {
        var query =  Query.Where<int>(a => a == deviceId, nameof(Violation.DeviceId));
        
        return query.ExecuteAsync();
    }
}
