using OpenLicenseManagementBL.DTOs;
using OpenLicenseServerDAL.Models;
using Infrastructure.Query;
using OpenLicenseManagementBL.DTOs.Query;

namespace OpenLicenseManagementBL.QueryObjects;
//taken from course FI:PV179 materials
public abstract class QueryObject<TEntity> where TEntity : BaseEntity, new()
{
    private readonly string _defaultSortCriteria;
    protected readonly IQuery<TEntity> Query;

    protected QueryObject(IQuery<TEntity> query, string defaultSortCriteria = nameof(BaseEntity.Id))
    {
        Query = query;
        _defaultSortCriteria = defaultSortCriteria;
    }

    protected IQuery<TEntity> ApplyPagination(IQuery<TEntity> query, FilterDto filterDto)
    {
        if (filterDto.PageNumber.HasValue)
        {
            query = filterDto.PageSize.HasValue
                ? query.Page(filterDto.PageNumber.Value, filterDto.PageSize.Value)
                : query.Page(filterDto.PageNumber.Value);
        }

        return query;
    }

    protected IQuery<TEntity> ApplySorting(IQuery<TEntity> query, FilterDto filterDto)
    {
        var sortCriteria = string.IsNullOrWhiteSpace(filterDto.SortCriteria)
            ? _defaultSortCriteria
            : filterDto.SortCriteria;

        if (!string.IsNullOrWhiteSpace(filterDto.SortCriteria))
        {
            query = query.OrderBy(sortCriteria, !filterDto.SortDescending);
        }

        return query;
    }
}