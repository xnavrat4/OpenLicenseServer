using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public interface IChallengeFacade
{
    Task CreateAsync(Challenge challenge);

    Task<Challenge?> GetByDeviceId(int deviceId);
    Task<bool> ResponseValidator(int deviceId, string response);
    
    Task DeleteAsync(int id);
}