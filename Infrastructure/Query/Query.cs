using OpenLicenseServerDAL.Models;
using System.Linq.Expressions;
using Castle.Components.DictionaryAdapter.Xml;
using Infrastructure.EFCore.Query;
//taken from course FI:PV179 materials
namespace Infrastructure.Query
{
    public abstract class Query<TEntity> : IQuery<TEntity> where TEntity : BaseEntity, new()
    {
        public IList<string> IncludeAttributes { get; set; }
        public List<(Expression expression, Type argumentType, string columnName)> WherePredicate { get; set; } = new();
        public (string columnName, bool isAscending)? OrderByContainer { get; set; }
        public (int PageToFetch, int? PageSize)? PaginationContainer { get; set; }

        public IQuery<TEntity> Page(int pageToFetch, int pageSize = 10)
        {
            PaginationContainer = (pageToFetch, pageSize);
            return this;
        }

        public IQuery<TEntity> OrderBy(string columnName, bool ascendingOrder = true)
        {
            OrderByContainer = (columnName, ascendingOrder);
            return this;
        }

        public IQuery<TEntity> Where<T>(Expression<Func<T, bool>> predicate, string columnName) where T : IComparable<T>
        {
            WherePredicate.Add((predicate, typeof(T), columnName));
            return this;
        }

        public abstract QueryResult<TEntity> Execute();
        public abstract Task<QueryResult<TEntity>> ExecuteAsync();
    }
}
