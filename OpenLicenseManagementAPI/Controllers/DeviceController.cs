using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenLicenseManagementBL.DTOs;
using OpenLicenseManagementBL.DTOs.Device;
using OpenLicenseManagementBL.DTOs.Query;
using OpenLicenseManagementBL.Facades;

namespace OpenLicenseManagementAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceFacade _deviceFacade;

    public DeviceController(IDeviceFacade deviceFacade, ILogger<DeviceController> logger)
    {
        _deviceFacade = deviceFacade;
        _logger = logger;
    }

/// <summary>
/// Gets device by id
/// </summary>
/// <param name="id">Id of a device</param>
/// <returns>A device in DeviceDetailDTO object format</returns>
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        var deviceDto = await _deviceFacade.GetDeviceByIdAsync(id);

        if (deviceDto == null)
        {
            return NotFound($"Device with id:{id} doesn't exist.");
        }
        
        return Ok(deviceDto);
    }
    
/// <summary>
/// Validates a device.
/// </summary>
/// <param name="deviceValidateDto">DTO of a device that is requested to be validated</param>
/// <returns>OK</returns>
    [Authorize]
    [HttpPut("Validate")]
    public async Task<IActionResult> ValidateDevice([FromBody] DeviceValidateDto deviceValidateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _deviceFacade.ValidateDeviceAsync(deviceValidateDto);
        
        _logger.LogInformation($"Successfully validated device: {deviceValidateDto.Id}");

        return Ok();
    }
   
/// <summary>
/// Resolves a violation with id and device id from resolveDto parameter
/// </summary>
/// <param name="resolveDto">ResolveDto contains a violationId and deviceID</param>
/// <returns>OK</returns>
    [Authorize] 
    [HttpPut("ResolveViolation")]
    public async Task<IActionResult> ResolveViolation([FromBody] ViolationResolveDto resolveDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _deviceFacade.ResolveViolationAsync(resolveDto);

        return Ok();
    }
    
/// <summary>
/// Updates a device.
/// </summary>
/// <param name="deviceDto">A DeviceDto object that is requested to be updated</param>
/// <returns>OK</returns>
    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] DeviceDto deviceDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        await _deviceFacade.UpdateDeviceAsync(deviceDto);
        return Ok();
    }
    
/// <summary>
/// Deletes a device and all their licenses.
/// </summary>
/// <param name="id">Id of a device</param>
/// <returns>OK</returns>
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        await _deviceFacade.DeleteDeviceAsync(id);
        return Ok();
    }

/// <summary>
/// Gets all (full-fledged) devices in DeviceDTO format.
/// </summary>
/// <returns>All devices</returns>
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _deviceFacade.GetAllDeviceAsync());
    }
    
/// <summary>
/// Gets Hardware Info about device with id.
/// </summary>
/// <param name="id">Id of a device</param>
/// <returns>HWInfo in HWInfoDto format</returns>
    [Authorize]
    [HttpGet("HWInfo/{id:int}")]
    public async Task<IActionResult> GetDeviceHWInfo([FromRoute] int id)
    {
        return Ok(await _deviceFacade.GetDeviceHWInfo(id));
    }

/// <summary>
/// Gets licenses that are bound to a device with id.
/// </summary>
/// <param name="id">Id of a device</param>
/// <returns>All licenses in LicenseDTO format</returns>
    [Authorize]
    [HttpGet("Licenses/{id:int}")]
    public async Task<IActionResult> GetLicensesOfDevice([FromRoute] int id)
    {
        return Ok(await _deviceFacade.GetDevicesLicenses(id));
    }
    
/// <summary>
/// Gets violations committed by a device with id.
/// </summary>
/// <param name="id">Id of a device</param>
/// <returns>All violations in ViolationDTO format</returns>
    [Authorize]
    [HttpGet("Violations/{id:int}")]
    public async Task<IActionResult> GetViolationsOfDevice([FromRoute] int id)
    {
        return Ok(await _deviceFacade.GetDevicesViolations(id));
    }

/// <summary>
/// Gets all devices filtered based on filterDto parameter
/// </summary>
/// <param name="filterDto">Filter parameters</param>
/// <returns>All Device Dtos that conform to the parameters</returns>
    [Authorize]
    [HttpGet ("Filter")]
    public async Task<IActionResult> GetAllFiltered([FromQuery] FilterDto filterDto)
    {
        return Ok(await _deviceFacade.GetFilteredDevices(filterDto));
    }

/// <summary>
/// Get all (simple) devices - only id and serial number attributes
/// </summary>
/// <returns>All devices as DeviceNameDto objects</returns>
    [Authorize]
    [HttpGet ("Simple")]
    public async Task<IActionResult> GetAllSimple()
    {
        return Ok(await _deviceFacade.GetAllDeviceSimpleAsync());
    }
    
/// <summary>
/// Gets all devices filtered based on filterDto parameter. It returns either validated or non validated devices.
/// </summary>
/// <param name="filterDto">Filter parameters</param>
/// <returns>All Device Dtos that conform to the parameters</returns>
    [Authorize]
    [HttpGet("Validity")]
    public async Task<IActionResult> GetAllFilteredValidity([FromQuery] DeviceValidityDto filterDto)
    {
        return Ok(await _deviceFacade.GetDevicesWithValidity(filterDto));
    }
}