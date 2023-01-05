using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IChallengeService
{
    Task CreateAsync(Challenge challenge);

    Task<Challenge?> GetById(int id);

    Task DeleteAsync(int id);
}