using OpenLicenseServerDAL.Models;
using Infrastructure.EFCore.Query;
using Infrastructure.Query;

namespace OpenLicenseServerBL.QueryObjects;

public class UserQueryObject : QueryObject<User>
{
    public UserQueryObject(IQuery<User> query) : base(query) { }

    public QueryResult<User> ExecuteQuery(string email)
    {
        var query = Query
            .Where<string>(a => a == email, nameof(User.Email));

        return query.Execute();
    }
}