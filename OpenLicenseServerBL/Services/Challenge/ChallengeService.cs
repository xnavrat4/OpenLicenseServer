using Infrastructure.Repository;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public class ChallengeService : IChallengeService
{
    private readonly IRepository<Challenge> _repository;

    public ChallengeService(IRepository<Challenge> repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(Challenge challenge)
    {
        await _repository.Insert(challenge);
        await _repository.Save();
    }

    public async Task<Challenge?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
        await _repository.Save();
    }
}