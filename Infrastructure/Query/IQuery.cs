using OpenLicenseServerDAL.Models;
using System.Linq.Expressions;
using Infrastructure.EFCore.Query;

namespace Infrastructure.Query
{
    public interface IQuery<TEntity> where TEntity : BaseEntity, new()
    {
        public IList<string> IncludeAttributes { get; set; }
        
        IQuery<TEntity> Where<T>(Expression<Func<T, bool>> rootPredicate, string columnName) where T : IComparable<T>;

        IQuery<TEntity> OrderBy(string columnName, bool ascendingOrder = true);

        IQuery<TEntity> Page(int pageToFetch, int pageSize = 10);

        QueryResult<TEntity> Execute();
    }
}
