using Infrastructure.Repository;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public class BaseService<TEntity> : IService<TEntity> where TEntity : BaseEntity
{
    private readonly IRepository<TEntity> _repository;

    public BaseService(IRepository<TEntity> repository)
    {
        _repository = repository;
    }
    public async ValueTask DisposeAsync()
    {
        await _repository.DisposeAsync();
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _repository.Insert(entity);
    }

    public async Task<TEntity?> GetById(int id)
    {
        return await _repository.GetById(id);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _repository.Update(entity);
        await _repository.Save();
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.Delete(id);
        await _repository.Save();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.Get();
    }
}