using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.ConnectionLog;
using OpenLicenseManagementBL.Facades;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ConnectionLogController : ControllerBase
{
    private readonly ILogger<ConnectionLogController> _logger;
    private readonly IConnectionLogFacade _connectionLogFacade;

    public ConnectionLogController(ILogger<ConnectionLogController> logger, IConnectionLogFacade connectionLogFacade)
    {
        _logger = logger;
        _connectionLogFacade = connectionLogFacade;
    }
  /// <summary>
  ///  Gets all Connection Log Dtos that conform to the parameters in filter dto 
  /// </summary>
  /// <param name="filterDto">Filter parameters</param>
  /// <returns>All Connection Log Dtos that conform to the parameters</returns>
    [Authorize]
    [HttpGet("Filter")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] ConnectionLogFilterDto filterDto)
    {
        return Ok(await _connectionLogFacade.GetAllFilteredLogsAsync(filterDto));
    }
    /// <summary>
    /// Gets all the violations of a device
    /// </summary>
    /// <param name="deviceId">Id of a target device</param>
    /// <returns>All violations of a device</returns>
    [Authorize]
    [HttpGet("Violations{deviceId:int}")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] int deviceId)
    {
        return Ok(await _connectionLogFacade.GetViolationsOfDevice(deviceId));
    }
    
}