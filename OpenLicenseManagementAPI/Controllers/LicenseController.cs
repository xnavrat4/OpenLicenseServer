using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.License;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.Facades;
using OpenLicenseServerBL.DTOs.Device;

namespace OpenLicenseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LicenseController : ControllerBase
{
    private readonly ILogger<LicenseController> _logger;
    private readonly ILicenseFacade _licenseFacade;

    public LicenseController(ILogger<LicenseController> logger, ILicenseFacade licenseFacade)
    {
        _logger = logger;
        _licenseFacade = licenseFacade;
    }
    
    /// <summary>
    /// Creates a new license
    /// </summary>
    /// <param name="license">A DTO object of a license</param>
    /// <returns>OK or BadRequest</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LicenseCreateDto license)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var licenseDto = await _licenseFacade.CreateLicenseAsync(license);

        return Ok(licenseDto);
    }
    
    /// <summary>
    /// Gets a license by their id
    /// </summary>
    /// <param name="id">Id of a license</param>
    /// <returns>A licenseDTO object or error that license with such id does not exist</returns>
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var license = await _licenseFacade.GetLicenseByIdAsync(id);

        if (license == null)
        {
            return NotFound($"License with id:{id} doesn't exist.");
        }
        
        return Ok(license);
    }
    
    /// <summary>
    /// Updates a license.
    /// </summary>
    /// <param name="license">A license object to be updated</param>
    /// <returns>OK</returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] LicenseDto license)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _licenseFacade.UpdateLicenseAsync(license);
        return Ok();
    }
    
    /// <summary>
    /// Deletes a license with a id.
    /// </summary>
    /// <param name="id">license's id</param>
    /// <returns>OK</returns>
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _licenseFacade.DeleteLicenseAsync(id);
        return Ok();
    }
  
    /// <summary>
    /// Gets all licenses
    /// </summary>
    /// <returns>All licenses as LicenseDto objects</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _licenseFacade.GetAllLicensesAsync());
    }
    
    /// <summary>
    /// Gets all licenses filtered based on filterDto parameter
    /// </summary>
    /// <param name="filterDto">Filter parameters</param>
    /// <returns>All License Dtos that conform to the parameters</returns>
    [Authorize]
    [HttpGet("Filter")]
    public async Task<IActionResult> GetFilteredLicensesAsync([FromQuery] FilterDto filterDto)
    {
        return Ok(await _licenseFacade.GetFilteredLicenses(filterDto));
    }
}