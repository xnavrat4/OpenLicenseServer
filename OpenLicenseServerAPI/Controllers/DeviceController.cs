using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenLicenseBL;
using OpenLicenseBL.DTOs;
using OpenLicenseServerBL.DTOs.Device;
using OpenLicenseServerBL.Facades;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
    private readonly ILogger<DeviceController> _logger;
    private readonly IDeviceFacade _deviceFacade;
    private readonly IChallengeFacade _challengeFacade;
    private readonly DataCrypter _dataCrypter;

    public DeviceController(IDeviceFacade deviceFacade, ILogger<DeviceController> logger,
        IChallengeFacade challengeFacade, DataCrypter dataCrypter)
    {
        _deviceFacade = deviceFacade;
        _logger = logger;
        _challengeFacade = challengeFacade;
        _dataCrypter = dataCrypter;
    }
    
/// <summary>
/// Registers a device and returns a challenge.
/// </summary>
/// <param name="deviceRegisterDto">DeviceregisterDTO</param>
/// <returns>challenge for device authentication</returns>
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] DeviceRegisterDto deviceRegisterDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }


        var foundDevices = await _deviceFacade.FindDeviceByHWInfoHash(deviceRegisterDto.HwInfo);

        //if there is no device, than create it!
        if (foundDevices.Item1 is null)
        { 
            var newDevice = await _deviceFacade.RegisterDeviceAsync(deviceRegisterDto, foundDevices.Item2);
            foundDevices = newDevice;
        }

        var device = foundDevices.Item1;
        
        if (device.Validated)
        {
            return BadRequest("Device already validated");
        }
        
        var deviceId = device.Id;
        var random = new Random().Next();
        var dbChallenge = await _challengeFacade.GetByDeviceId(deviceId);
        if (dbChallenge != null)
        {
            return BadRequest("Challenge already in process");
        }

        var challenge = new Challenge() { DeviceId = deviceId, Value = random };
        await _challengeFacade.CreateAsync(challenge);
        return Ok(new ChallengeDto() { Random = random, DeviceId = deviceId });
    }


/// <summary>
/// Validates the response from a device and validates it if the response is correct
/// </summary>
/// <param name="responseDto">ResponseDto containing the device id and response</param>
/// <returns>OK with serverTime, device's PK and its signature</returns>
    [HttpPost("Challenge")]
    public async Task<IActionResult> ChallengeAsync([FromBody] ChallengeResponseDto responseDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var challengePassed = await _challengeFacade.ResponseValidator(responseDto.DeviceId, responseDto.Response);

        if (!challengePassed)
        {
            _logger.LogError($"Challenge failed by device {responseDto.DeviceId}");
            return BadRequest("Challenge failed");
        }

        await _deviceFacade.ValidateDeviceAsync(responseDto.DeviceId);
        var dev = await _deviceFacade.GetById(responseDto.DeviceId);

        var pk = dev?.PublicKey!;
        var data = _dataCrypter.Sign(pk);
        var rval = new { PublicKey = pk, Signature = data, DateTime = DateTime.UtcNow };
        _logger.LogInformation($"Device {responseDto.DeviceId} has passed the challenge");
        return Ok(rval);
    }

/// <summary>
/// Report a device. 
/// </summary>
/// <param name="secretDto">SecretDto containing reportrequestDTO</param>
/// <returns>A report object of badRequest</returns>
    [HttpPost("Report")]
    public async Task<IActionResult> ReportAsync([FromBody] SecretDto secretDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var keyAndIV = _dataCrypter.GetKeyAndIV(secretDto.Key);
        var decrypted = _dataCrypter.Decrypt(secretDto.Secret, keyAndIV);
        ReportRequestDto? reportRequestDto = JsonConvert.DeserializeObject<ReportRequestDto>(decrypted);

        if (reportRequestDto == null)
        {
            return BadRequest("Deserialization failed");
        }

        var device = await _deviceFacade.GetById(reportRequestDto.DeviceId);
        if (device == null)
        {
            return BadRequest($"Device with id: {reportRequestDto.DeviceId} does not exist");
        }
        
        var report = await _deviceFacade.ReportDevice(reportRequestDto);
        var reportJSON = JsonConvert.SerializeObject(report);
        var publicKey = await _deviceFacade.GetDevicePublicKey(reportRequestDto.DeviceId);
        var encrypted = DataCrypter.Encrypt(reportJSON, publicKey, keyAndIV);
        return Ok(encrypted);
    }
}