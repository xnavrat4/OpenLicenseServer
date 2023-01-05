using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.QueryObjects;

public class CustomersQueryObject: QueryObject<Customer>
{
    
        public CustomersQueryObject(IQuery<Customer> query) : base(query) { 
            query.IncludeAttributes = new List<string>() {nameof(Customer.Devices)};
            
        }
    
        public Task<QueryResult<Customer>> ExecuteQueryAsync(FilterDto filterDto)
        {
            var query = Query;

            query = ApplySorting(query, filterDto);
        
            query = ApplyPagination(query, filterDto);
            return query.ExecuteAsync();
        }
    
}
