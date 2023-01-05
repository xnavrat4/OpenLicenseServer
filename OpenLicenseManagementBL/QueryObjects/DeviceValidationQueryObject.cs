using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.QueryObjects;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

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
            .Where<bool>(a => a == filterDto.Activated, nameof(Device.Activated));
        
        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
