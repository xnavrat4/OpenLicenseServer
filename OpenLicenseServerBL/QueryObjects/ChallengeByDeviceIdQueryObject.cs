using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class ChallengeByDeviceIdQueryObject : QueryObject<Challenge>
{
    public ChallengeByDeviceIdQueryObject(IQuery<Challenge> query, string defaultSortCriteria = "Id") : base(query, defaultSortCriteria)
    {
    }
    
    public Task<QueryResult<Challenge>> ExecuteQueryAsync(int deviceId)
    {
        var query = Query.Where<int>(a => a == deviceId, nameof(Challenge.DeviceId));
        
        return query.ExecuteAsync();
    }
}
