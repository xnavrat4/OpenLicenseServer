using OpenLicenseServerBL.DTOs;
using OpenLicenseServerDAL.Models;

namespace OpenLicenseServerBL.Services;

public interface IService<TEntity> : IAsyncDisposable where TEntity : BaseEntity   
{
    Task CreateAsync(TEntity baseDto);

    Task<TEntity?> GetById(int id);

    Task UpdateAsync(TEntity baseDto);

    Task DeleteAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync();

}