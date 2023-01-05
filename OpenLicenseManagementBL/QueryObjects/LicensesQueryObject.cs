using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class LicensesQueryObject : QueryObject<License>
{
    public LicensesQueryObject(IQuery<License> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria) {
    }


    public Task<QueryResult<License>> ExecuteQueryAsync(FilterDto filterDto)
    {
        var query = Query;

        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
