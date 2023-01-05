using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class DeviceQueryObject : QueryObject<Device>
{
    public DeviceQueryObject(IQuery<Device> query) : base(query) { }
    
    public Task<QueryResult<Device>> ExecuteQueryAsync(FilterDto filterDto)
    {
        var query = Query;

        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
