using Infrastructure.Repository;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseManagementBL.Services;

public class ViolationService : IViolationService
{
    private readonly IRepository<Violation> _repository;

    public ViolationService(IRepository<Violation> repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(Violation violation)
    {
        await _repository.Insert(violation);
        await _repository.Save();
    }

    public async Task<Violation?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task UpdateAsync(Violation violation)
    {
        _repository.Update(violation);
        await _repository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
    }

    public async Task<IEnumerable<Violation>> GetAllAsync()
    {
        return await _repository.Get();
    }
}