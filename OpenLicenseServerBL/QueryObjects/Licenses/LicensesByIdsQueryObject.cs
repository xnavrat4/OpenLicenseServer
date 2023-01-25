using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects.Licenses;

public class LicensesByIdsQueryObject : QueryObject<License>
{
    public LicensesByIdsQueryObject(IQuery<License> query) : base(query) {
        //query.IncludeAttributes = new List<string>(){nameof(Restaurant.RestaurantRatings), nameof(Restaurant.Address), nameof(Restaurant.RestaurantCategories)};
    }


    public QueryResult<License> ExecuteQuery(IEnumerable<int> licenseIds)
    {
        var query = Query.Where<int>(a => licenseIds.Contains(a), nameof(License.Id));
        return query.Execute();
    }
}