using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class DeviceHWInfoHashQueryObject : QueryObject<Device>
{
    public DeviceHWInfoHashQueryObject(IQuery<Device> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public QueryResult<Device> ExecuteQuery(DeviceHWInfoHash filterDto)
    {
        var query = Query.Where<string>(a => a == filterDto.HWInfoHash, nameof(Device.HWInfoHash));
        
        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.Execute();
    }
}