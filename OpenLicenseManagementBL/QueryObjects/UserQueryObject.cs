using OpenLicenseServerDAL.Models;
using Infrastructure.EFCore.Query;
using Infrastructure.Query;

namespace OpenLicenseManagementBL.QueryObjects;

public class UserQueryObject : QueryObject<User>
{
    public UserQueryObject(IQuery<User> query) : base(query) { }

    public Task<QueryResult<User>> ExecuteQueryAsync(string email)
    {
        var query = Query
            .Where<string>(a => a == email, nameof(User.Email));

        return query.ExecuteAsync();
    }
}
