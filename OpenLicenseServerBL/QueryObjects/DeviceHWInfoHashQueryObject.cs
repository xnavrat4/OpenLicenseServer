using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class DeviceHWInfoHashQueryObject : QueryObject<Device>
{
    public DeviceHWInfoHashQueryObject(IQuery<Device> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public Task<QueryResult<Device>> ExecuteQueryAsync(DeviceHWInfoHashFilterDto filterDto)
    {
        var query = Query.Where<string>(a => a == filterDto.HWInfoHash, nameof(Device.HWInfoHash));

        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
