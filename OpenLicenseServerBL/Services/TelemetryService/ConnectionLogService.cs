using Infrastructure.Repository;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public class ConnectionLogService : IConnectionLogService
{
    private readonly IRepository<ConnectionLog> _repository;

    public ConnectionLogService(IRepository<ConnectionLog> repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(ConnectionLog log)
    {
        await _repository.Insert(log);
        await _repository.Save();
    }

    public async Task<ConnectionLog?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task UpdateAsync(ConnectionLog license)
    {
        _repository.Update(license);
        await _repository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
    }

    public async Task<IEnumerable<ConnectionLog>> GetAllAsync()
    {
        return await _repository.Get();
    }
}