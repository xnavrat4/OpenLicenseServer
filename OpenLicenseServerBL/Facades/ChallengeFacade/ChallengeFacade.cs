using System.Security.Cryptography;
using System.Text;
using Infrastructure.UnitOfWork;
using OpenLicenseServerBL.QueryObjects;
using OpenLicenseServerBL.Services;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Facades;

public class ChallengeFacade: IChallengeFacade
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IChallengeService _challengeService;
    private readonly ChallengeByDeviceIdQueryObject _challengeByDeviceIdQueryObject;

    public ChallengeFacade(IChallengeService challengeService, IUnitOfWork unitOfWork, ChallengeByDeviceIdQueryObject challengeByDeviceIdQueryObject)
    {
        _challengeService = challengeService;
        _unitOfWork = unitOfWork;
        _challengeByDeviceIdQueryObject = challengeByDeviceIdQueryObject;
    }

    public async Task CreateAsync(Challenge challenge)
    {
        await _challengeService.CreateAsync(challenge);
        await _unitOfWork.CommitAsync();
    }

    public async Task<Challenge?> GetByDeviceId(int deviceId)
    {
        var qr = await _challengeByDeviceIdQueryObject.ExecuteQueryAsync(deviceId);
        return qr.Items?.FirstOrDefault();
    }

    public async Task DeleteAsync(int id)
    {
        await _challengeService.DeleteAsync(id);
        await _unitOfWork.CommitAsync();
    }
    
    public async Task<bool> ResponseValidator(int deviceId, string response)
    {
        
        var challenge = await GetByDeviceId(deviceId);
        if (challenge is null)
        {
            return false;
        }

        var correctResponse = CalculateResponse(challenge.Value);
        await DeleteAsync(challenge.Id);
        return correctResponse == response.ToUpper();
    }

    private string CalculateResponse(long rand)
    {
        rand /= 7;
        rand *= 7;
        
        byte[] hash;
        using (SHA512 shaM = new SHA512Managed())
        {
            var bytes = Encoding.ASCII.GetBytes(rand.ToString());
            hash = shaM.ComputeHash(bytes);
        }

        string response = "";
        foreach (var oneByte in hash)
        {
            var array = new byte[]{oneByte};
            response += Convert.ToHexString(array);
        }

        return response;
    }
}
