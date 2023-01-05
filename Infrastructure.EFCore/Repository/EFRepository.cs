using Castle.Core.Internal;
using OpenLicenseServerDAL.Data;
using OpenLicenseServerDAL.Models;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFCore.Repository;
//taken from course FI:PV179 materials
public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private OLSDbContext _dbContext;
    private DbSet<TEntity> _dbSet;

    public EFRepository(OLSDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<IEnumerable<TEntity>> Get(params string[] includes)
    {
        var queryable = _dbSet.AsQueryable();
        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include($"{include}");
            }
        }
        return await queryable.ToListAsync();
    }

    public async Task<TEntity?> GetById(int id, params string[] includes)
    {
        var queryable = _dbSet.AsQueryable();
        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                queryable = queryable.Include($"{include}");
            }
        }

        return await queryable.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task Insert(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task Delete(int id)
    {
        var entity = await _dbSet.FindAsync(id);

        if (_dbContext.Entry(entity).State == EntityState.Detached)
        {
            _dbContext.Attach(entity);
        }

        _dbSet.Remove(entity);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    /*public void Dispose()
    {
        _dbContext.Dispose();
    }*/
}
