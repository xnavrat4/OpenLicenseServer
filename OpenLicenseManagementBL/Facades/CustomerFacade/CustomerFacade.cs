using AutoMapper;
using Infrastructure.EFCore.Query;
using Infrastructure.UnitOfWork;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.Device;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.Services;
using OpenLicenseServerDAL.Models;
using CustomerNameDto = OpenLicenseManagementBL.DTOs.Customer.CustomerNameDto;

namespace OpenLicenseManagementBL.Facades;

public class CustomerFacade : ICustomerFacade
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICustomerService _customerService;

    public CustomerFacade(IMapper mapper, IUnitOfWork unitOfWork, ICustomerService customerService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _customerService = customerService;
    }

    public async Task CreateCustomerAsync(CustomerCreateDto customerCreateDto)
    {
        var customer = _mapper.Map<Customer>(customerCreateDto);
        await _customerService.CreateAsync(customer);
        await _unitOfWork.CommitAsync();
    }

    public async Task<CustomerDetailDto?> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _customerService.GetById(customerId);
        var fulldevs = new List<Device>();
        foreach (var device in customer.Devices)
        {
            fulldevs.Add(await _unitOfWork.DeviceRepository.GetById(device.Id, new []{nameof(Device.Licenses)}));
        }

        customer.Devices = fulldevs;
        return _mapper.Map<CustomerDetailDto>(customer);
    }

    public async Task UpdateCustomerAsync(CustomerUpdateDto customerTableDto)
    {
        var customer = _mapper.Map<Customer>(customerTableDto);
        await _customerService.UpdateAsync(customer);
    }

    public async Task<IEnumerable<CustomerTableDto>> GetAllCustomersAsync()
    {
        var customers = await _customerService.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerTableDto>>(customers);
    }

    public async Task DeleteCustomerAsync(int customerId)
    {
        await _customerService.DeleteAsync(customerId);
    }

    public async Task<QueryResult<CustomerTableDto>> GetFilteredCustomers(FilterDto filterDto)
    {
        var queryResult = await _customerService.GetFilteredCustomers(filterDto);
        return _mapper.Map<QueryResult<CustomerTableDto>>(queryResult);
    }

    public async Task<IEnumerable<CustomerNameDto>> GetSimpleCustomersAsync()
    {
        var customers = await _customerService.GetAllAsync();
        return _mapper.Map<IEnumerable<CustomerNameDto>>(customers);
    }
}