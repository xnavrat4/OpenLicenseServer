using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class LicenseKeyQueryObject : QueryObject<License>
{
    public LicenseKeyQueryObject(IQuery<License> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
        query.IncludeAttributes = new List<string>()
            {nameof(License.Device)};
    }
    
    public Task<QueryResult<License>> ExecuteQueryAsync(LicenseKeyFilterDto filterDto)
    {
        var query = Query.Where<Guid>(g => g == filterDto.LicenseKey, nameof(License.LicenseKey));

        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
