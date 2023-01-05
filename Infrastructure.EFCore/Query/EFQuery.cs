using Infrastructure.Query;
using System.Linq.Expressions;
using OpenLicenseServerDAL.Data;
using OpenLicenseServerDAL.Models;
using Infrastructure.EFCore.ExpressionHelpers;
using Infrastructure.EFCore.UnitOfWork;
using Microsoft.EntityFrameworkCore;
//taken from course FI:PV179 materials
namespace Infrastructure.EFCore.Query
{
    public class EFQuery<TEntity> : Query<TEntity> where TEntity : BaseEntity, new()
    {
        private OLSDbContext _dbContext;

        private EFUnitOfWork _unitOfWork;
        
        public EFQuery(OLSDbContext dbContext)
        {
            _dbContext = dbContext;
            IncludeAttributes = new List<string>();
        }

        public override QueryResult<TEntity> Execute()
        {
            return ExecuteInternal(false).Result;
        }

        public override Task<QueryResult<TEntity>> ExecuteAsync()
        {
            return ExecuteInternal(true);
        }

        private async Task<QueryResult<TEntity>> ExecuteInternal(bool async)
        {
            IQueryable<TEntity> query = _dbContext.Set<TEntity>();

            if (WherePredicate.Capacity != 0)
            {
                query = ApplyWhere(query);
            }

            var totalCount = query.Count();

            if (OrderByContainer != null)
            {
                query = OrderBy(query);
            }

            if (PaginationContainer.HasValue)
            {
                query = Pagination(query);
            }

            query = IncludeAttributes.Aggregate(query, (current, at) => current.Include(at));

            var result = await (async ? query.ToListAsync() : Task.FromResult(query.ToList())).ConfigureAwait(false);

            if (PaginationContainer is { PageToFetch: > 0 })
            {
                var pagination = PaginationContainer.Value;
                return new QueryResult<TEntity>(result, totalCount, pagination.PageSize ?? totalCount, pagination.PageToFetch);
            }

            return new QueryResult<TEntity>(result, totalCount);
        }

        private IQueryable<TEntity> ApplyWhere(IQueryable<TEntity> query)
        {
            foreach (var expr in WherePredicate)
            {
                var p = Expression.Parameter(typeof(TEntity), "p");

                var columnNameFromObject = typeof(TEntity)
                    .GetProperty(expr.columnName)
                    ?.Name;

                var exprProp = Expression.Property(p, columnNameFromObject);

                var expression = expr.expression;

                var parameters = (IReadOnlyCollection<ParameterExpression>)expression
                    .GetType()
                    .GetProperty("Parameters")
                    ?.GetValue(expression);

                var body = (Expression)expression
                    .GetType()
                    .GetProperty("Body")
                    ?.GetValue(expression);

                var visitor = new ReplaceParamVisitor(parameters.First(), exprProp);
                var exprNewBody = visitor.Visit(body);

                var lambda = Expression.Lambda<Func<TEntity, bool>>(exprNewBody, p);

                query = query.Where(lambda);
            }

            return query;
        }

        private IQueryable<TEntity> OrderBy(IQueryable<TEntity> query)
        {
            var orderByColumn = OrderByContainer.Value.columnName;
            var isAscending = OrderByContainer.Value.isAscending;

            var p = Expression.Parameter(typeof(TEntity), "p");

            var columnFromObject = typeof(TEntity).GetProperty(orderByColumn);

            var exprProp = Expression.Property(p, columnFromObject?.Name);
            var lambda = Expression.Lambda(exprProp, p);

            var orderByMethod = typeof(Queryable)
                .GetMethods()
                .First(a => a.Name == (isAscending ? "OrderBy" : "OrderByDescending") && a.GetParameters().Length == 2);

            var orderByClosedMethod = orderByMethod.MakeGenericMethod(typeof(TEntity), columnFromObject?.PropertyType);

            return (IQueryable<TEntity>)orderByClosedMethod.Invoke(null, new object[] { query, lambda })!;
        }

        private IQueryable<TEntity> Pagination(IQueryable<TEntity> query)
        {
            var page = PaginationContainer.Value.PageToFetch;
            var pageSize = PaginationContainer.Value.PageSize;

            var totalCount = query.Count();
            return query
                .Skip(page > 0 && pageSize.HasValue ? (page - 1) * pageSize.Value : 0)
                .Take(pageSize ?? totalCount);
        }
    }
}
