using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerBL.Facades;

namespace OpenLicenseServerAPI.Controllers;

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
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] LicenseCreateDto license)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _licenseFacade.CreateLicenseAsync(license);

        return Ok();
    }
    
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
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _licenseFacade.DeleteLicenseAsync(id);
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _licenseFacade.GetAllLicensesAsync());
    }
}