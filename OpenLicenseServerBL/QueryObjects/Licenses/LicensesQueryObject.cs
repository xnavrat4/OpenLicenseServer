using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class LicensesQueryObject : QueryObject<License>
{
    public LicensesQueryObject(IQuery<License> query) : base(query)
    {
        query.IncludeAttributes = new List<string>() { nameof(License.Device) };
    }


    public Task<QueryResult<License>> ExecuteQueryAsync(FilterDto filterDto)
    {
        var query = Query;

        query = ApplySorting(query, filterDto);

        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
