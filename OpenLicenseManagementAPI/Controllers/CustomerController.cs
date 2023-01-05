using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Customer;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.Facades;

namespace OpenLicenseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ILogger<CustomerController> _logger;
    private readonly ICustomerFacade _customerFacade;

    public CustomerController(ILogger<CustomerController> logger, ICustomerFacade customerFacade)
    {
        _logger = logger;
        _customerFacade = customerFacade;
    }
    /// <summary>
    /// Creates a new customer
    /// </summary>
    /// <param name="customerCreateDto">A DTO object of a customer</param>
    /// <returns>OK or BadRequest</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CustomerCreateDto customerCreateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _customerFacade.CreateCustomerAsync(customerCreateDto);

        return Ok();
    }
    /// <summary>
    /// Gets a customer by their id
    /// </summary>
    /// <param name="id">Id of a customer</param>
    /// <returns>A customerDTO object or error that customer with such id does not exist</returns>
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var customerDto = await _customerFacade.GetCustomerByIdAsync(id);

        if (customerDto == null)
        {
            return NotFound($"Customer with id:{id} doesn't exist.");
        }
        
        return Ok(customerDto);
    }
    /// <summary>
    /// Updates a customer.
    /// </summary>
    /// <param name="customerUpdateDto">A customer object to be updated</param>
    /// <returns>OK</returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CustomerUpdateDto customerUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _customerFacade.UpdateCustomerAsync(customerUpdateDto);
        return Ok();
    }
    /// <summary>
    /// Deletes a customer with a id. It also deletes all of their device and licenses.
    /// </summary>
    /// <param name="id">customer's id</param>
    /// <returns>OK</returns>
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _customerFacade.DeleteCustomerAsync(id);
        return Ok();
    }
    
    /// <summary>
    /// Gets all (full-fledged) customers
    /// </summary>
    /// <returns>All customers as CustomerTableDTO objects</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _customerFacade.GetAllCustomersAsync());
    }

    /// <summary>
    /// Get all (simple) customers - only id and name attributes
    /// </summary>
    /// <returns>All customers as CustomerNameDTO objects</returns>
    [Authorize]
    [HttpGet("Simple")]
    public async Task<IActionResult> GetAllSimpleAsync()
    {
        return Ok(await _customerFacade.GetSimpleCustomersAsync());
    }
    
    /// <summary>
    /// Gets all customers filtered based on filterDto parameter
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>All Customers Dtos that conform to the parameters</returns>
    [Authorize]
    [HttpGet("Filter")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] FilterDto filterDto)
    {
        return Ok(await _customerFacade.GetFilteredCustomers(filterDto));
    }
}