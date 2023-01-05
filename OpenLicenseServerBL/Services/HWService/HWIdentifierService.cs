using Infrastructure.Repository;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public class HWIdentifierService : IHWIdentifierService
{
    private IRepository<HWInfo> _repository;

    public HWIdentifierService(IRepository<HWInfo> repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(HWInfo hwInfo)
    {
        await _repository.Insert(hwInfo);
    }

    public async Task<HWInfo?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task UpdateAsync(HWInfo hwInfo)
    {
        _repository.Update(hwInfo);
        await _repository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
    }

    public async Task<IEnumerable<HWInfo>> GetAllAsync()
    {
        return await _repository.Get();
    }
}