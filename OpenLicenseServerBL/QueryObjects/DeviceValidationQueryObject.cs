using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class DeviceValidationQueryObject : QueryObject<Device>
{
    public DeviceValidationQueryObject(IQuery<Device> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
        query.IncludeAttributes = new List<string>()
            {nameof(Device.HwInfo), nameof(Device.Licenses)};
    }
    
    public Task<QueryResult<Device>> ExecuteQueryAsync(DeviceValidityDto filterDto)
    {
        var query = Query
            .Where<bool>(a => a == filterDto.Activated, nameof(Device.Activated))
            .Where<bool>(a => a == true, nameof(Device.Validated));
        
        
        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
