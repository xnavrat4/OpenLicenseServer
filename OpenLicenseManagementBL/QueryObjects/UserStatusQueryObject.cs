using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class UserStatusQueryObject : QueryObject<User>
{
    public UserStatusQueryObject(IQuery<User> query) : base(query) {
    }
    
    public Task<QueryResult<User>> ExecuteQueryAsync(UserStatusFilterDto filterDto)
    {
        var status = (int)filterDto.UserStatus;
        var query =  Query.Where<int>(a => a == status, nameof(User.UserStatus));


        query = ApplySorting(query, filterDto);
        
        query = ApplyPagination(query, filterDto);
        return query.ExecuteAsync();
    }
}
