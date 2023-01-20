using OpenLicenseServerDAL.Models;

namespace Infrastructure.Repository;

public interface IRepository<TEntity> : IAsyncDisposable where TEntity : BaseEntity
{
    Task<IEnumerable<TEntity>> Get(params string[] includes);

    Task<TEntity?> GetById(int id, params string[] includes);

    Task Insert(TEntity entity);

    void Update(TEntity entity);
    
    Task Delete(int id);

    Task Save();
}