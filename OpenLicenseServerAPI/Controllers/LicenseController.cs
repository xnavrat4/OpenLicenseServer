using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _licenseFacade.GetAllLicensesAsync());
    }
}