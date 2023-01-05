using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.Query;

namespace OpenLicenseManagementBL.Facades;

public interface ICustomerFacade
{
    Task CreateCustomerAsync(CustomerCreateDto customerCreateDto);
    
    Task<CustomerDetailDto?> GetCustomerByIdAsync(int customerId);
    
    Task UpdateCustomerAsync(CustomerUpdateDto customerTableDto);
    
    Task<IEnumerable<CustomerTableDto>> GetAllCustomersAsync();
    
    Task DeleteCustomerAsync(int customerId);

    Task<QueryResult<CustomerTableDto>> GetFilteredCustomers(FilterDto filterDto);
    Task<IEnumerable<CustomerNameDto>> GetSimpleCustomersAsync();
}