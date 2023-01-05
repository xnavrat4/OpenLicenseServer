using Infrastructure.EFCore.Query;
using Infrastructure.Query;
using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.QueryObjects;

public class CustomersQueryObject: QueryObject<Customer>
{
    
        public CustomersQueryObject(IQuery<Customer> query) : base(query) { 
            query.IncludeAttributes = new List<string>()
                {nameof(Customer.Devices)};
            
        }
    
        public QueryResult<Customer> ExecuteQuery(FilterDto filterDto)
        {
            var query = Query;

            query = ApplySorting(query, filterDto);
        
            query = ApplyPagination(query, filterDto);
            return query.Execute();
        }
    
}