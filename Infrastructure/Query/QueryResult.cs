using OpenLicenseServerDAL.Models;
//taken from course FI:PV179 materials
namespace Infrastructure.EFCore.Query
{
    public class QueryResult<TEntity>
    {
        public long TotalItemsCount { get; }
        public int? RequestedPageNumber { get; }
        public int? PageSize { get; }
        public IEnumerable<TEntity> Items { get; set; }

        public QueryResult(IEnumerable<TEntity> items, long totalItemsCount, int? pageSize = null, int? requestedPageNumber = null)
        {
            TotalItemsCount = totalItemsCount;
            RequestedPageNumber = requestedPageNumber;
            PageSize = pageSize;
            Items = items;
        }
    }
}
