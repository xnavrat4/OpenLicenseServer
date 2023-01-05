using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenLicenseBL.DTOs;
using OpenLicenseBL.Utils;
using OpenLicenseServerBL.DTOs.ConnectionLog;
using OpenLicenseServerBL.DTOs.License;
using OpenLicenseServerBL.Facades;

namespace OpenLicenseServerAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LicenseController : ControllerBase
{
    private readonly ILogger<LicenseController> _logger;
    private readonly ILicenseFacade _licenseFacade;
    private readonly IDeviceFacade _deviceFacade;
    private readonly IConnectionLogFacade _connectionLogFacade;
    private readonly DataCrypter _dataCrypter;

    public LicenseController(ILicenseFacade licenseFacade, ILogger<LicenseController> logger, IConnectionLogFacade connectionLogFacade, IDeviceFacade deviceFacade, DataCrypter dataCrypter)
    {
        _licenseFacade = licenseFacade;
        _logger = logger;
        _connectionLogFacade = connectionLogFacade;
        _deviceFacade = deviceFacade;
        _dataCrypter = dataCrypter;
    }
    /// <summary>
    /// Is tasked with returning either a license object or bad request
    /// </summary>
    /// <param name="secretDto">Encryped object containing licensevalidateDTO</param>
    /// <returns>Either a license object or bad request</returns>
    [HttpPut]
    public async Task<IActionResult> GetLicenseValid([FromBody] SecretDto secretDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var keyAndIV = _dataCrypter.GetKeyAndIV(secretDto.Key);
        var decrypted = _dataCrypter.Decrypt(secretDto.Secret, keyAndIV);
        var validateDto = JsonConvert.DeserializeObject<LicenseValidateDto>(decrypted);

        if (validateDto == null)
        {
            return BadRequest("Error while deserializing");
        }
        //check for device validity

        var device = await _deviceFacade.GetById(validateDto.DeviceId);
        
        if (device == null || !device.Validated || !device.Activated)
        {
            _logger.LogInformation($"Non validated device {device.Id} requested license");
            var encryptedResponse = DataCrypter.Encrypt("Device is not validated", device.PublicKey,keyAndIV);
            
            return BadRequest(encryptedResponse);
        }

        if (!Guid.TryParse(validateDto.LicenseKey, out var newGuid))
        {
            _logger.LogInformation($"Invalid key format");
            var encryptedResponse = DataCrypter.Encrypt("Device is not validated", device.PublicKey,keyAndIV);
            
            return BadRequest(encryptedResponse);
        }
        
        var licensesWithLicenseKey = await _licenseFacade.GetLicenseByLicenseKey(validateDto.LicenseKey);

        if (!licensesWithLicenseKey.Any())
        {
            var logDto = new ConnectionLogCreateDto()
            {
                DeviceId = validateDto.DeviceId, LicenseKey = new Guid(validateDto.LicenseKey), Result = RequestStateEnum.InvalidKey,
                Type = ConnectionLogEnum.LicenseRequest, SystemTime = DateTime.UtcNow
            };
            await _connectionLogFacade.CreateLogAsync(logDto);
            
            _logger.LogError($"No license with given serial number ${validateDto.LicenseKey} was found for device {device.Id}");
            var encryptedResponse = DataCrypter.Encrypt("Device is not validated", device.PublicKey,keyAndIV);
            return BadRequest(encryptedResponse);
        }
        var license = licensesWithLicenseKey.First();
        
        if (license.DeviceId == null)
        {
            var logDto = new ConnectionLogCreateDto()
            {
                DeviceId = validateDto.DeviceId, LicenseKey = new Guid(validateDto.LicenseKey), Result = RequestStateEnum.Assigned,
                Type = ConnectionLogEnum.LicenseRequest, SystemTime = DateTime.UtcNow
            };
            await _connectionLogFacade.CreateLogAsync(logDto);

            var newLicenseDto = await _licenseFacade.AssignLicenseToDevice(license, validateDto.DeviceId);
            _logger.LogInformation($"License with license key: ${validateDto.LicenseKey} was assigned to device {device.Id}");
            var newLicenseJson = JsonConvert.SerializeObject(newLicenseDto);
            var devicePublicKey = await _deviceFacade.GetDevicePublicKey(validateDto.DeviceId);
            
            return Ok(DataCrypter.Encrypt(newLicenseJson, devicePublicKey, keyAndIV));
        }
        
        if (license.DeviceId != validateDto.DeviceId)
        {
            var logDto = new ConnectionLogCreateDto()
            {
                DeviceId = validateDto.DeviceId, LicenseKey = new Guid(validateDto.LicenseKey), Result = RequestStateEnum.WrongDevice,
                Type = ConnectionLogEnum.LicenseRequest, SystemTime = DateTime.UtcNow
            };
            await _connectionLogFacade.CreateLogAsync(logDto);
            _logger.LogError($"License with serial number ${validateDto.LicenseKey} is bound to a different device than device {device.Id}");
            var encryptedResponse = DataCrypter.Encrypt($"License with serial number ${validateDto.LicenseKey} is bound to a different device", device.PublicKey,keyAndIV);
            return BadRequest(encryptedResponse);
        }
        
        var validDto = new ConnectionLogCreateDto()
        {
            DeviceId = validateDto.DeviceId, LicenseKey = new Guid(validateDto.LicenseKey), Result = RequestStateEnum.Granted,
            Type = ConnectionLogEnum.LicenseRequest, SystemTime = DateTime.UtcNow
        };
        await _connectionLogFacade.CreateLogAsync(validDto);
        
        var licenseDto = _licenseFacade.ReturnLicenseToDevice(license);
        var json = JsonConvert.SerializeObject(licenseDto);
        var publicKey = await _deviceFacade.GetDevicePublicKey(validateDto.DeviceId);
        var encrypted = DataCrypter.Encrypt(json, publicKey,keyAndIV);
        
        return Ok(encrypted);
    }
}