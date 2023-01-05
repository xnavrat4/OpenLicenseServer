namespace OpenLicenseManagementBL.DTOs.Query;

public class QueryResultDto<TDto>
{
    public QueryResultDto()
    {
        Items = new List<TDto>();
    }

    public long TotalItemsCount { get; set; }
    public int? RequestedPageNumber { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<TDto> Items { get; set; }
}