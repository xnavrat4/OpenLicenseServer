using Infrastructure.EFCore.Query;
using Infrastructure.Repository;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.QueryObjects;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;
    private readonly CustomersQueryObject _queryObject;

    public CustomerService(IRepository<Customer> repository, CustomersQueryObject queryObject)
    {
        _repository = repository;
        _queryObject = queryObject;
    }

    public async Task CreateAsync(Customer customer)
    {
        await _repository.Insert(customer);
    }

    public async Task<Customer?> GetById(int id)
    {
        return await _repository.GetById(id, new []{nameof(Customer.Devices)});
    }

    public async Task UpdateAsync(Customer customer)
    {
        _repository.Update(customer);
        await _repository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
        await _repository.Save();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _repository.Get();
    }

    public Task<QueryResult<Customer>> GetFilteredCustomers(FilterDto filterDto)
    {
        return _queryObject.ExecuteQueryAsync(filterDto);
    }
}
