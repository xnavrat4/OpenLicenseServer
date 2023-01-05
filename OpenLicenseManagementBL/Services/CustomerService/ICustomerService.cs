using Infrastructure.EFCore.Query;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Services;

public interface ICustomerService
{
    Task CreateAsync(Customer customer);

    Task<Customer?> GetById(int id);

    Task UpdateAsync(Customer customer);

    Task DeleteAsync(int id);

    Task<IEnumerable<Customer>> GetAllAsync();

    Task<QueryResult<Customer>> GetFilteredCustomers(FilterDto filterDto);
}