﻿using Microsoft.EntityFrameworkCore;

namespace Contracts.Abstractions.Paging;

public record PagedResult<T>
{
    public const int UpperPageSize = 100;
    public const int DefaultPageSize = 10;
    public const int DefaultPageIndex = 1;

    private PagedResult(List<T> items, int pageIndex, int pageSize, int totalCount)
    {
        Items = items;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
    }

    public List<T> Items { get; }
    public int PageIndex { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex * PageSize < TotalCount;

    public static PagedResult<T> Create(List<T> items, int pageIndex, int pageSize, int totalCount)
        => new(items, pageIndex, pageSize, totalCount);

    public static async Task<PagedResult<T>> CreateAsync(
        IQueryable<T> query, 
        int pageIndex, 
        int pageSize, 
        CancellationToken cancellationToken)
    {
        //pageIndex = pageIndex <= 0 ? DefaultPageIndex : pageIndex;
        //pageSize = pageSize <= 0
        //    ? DefaultPageSize
        //    : pageSize > 100
        //    ? UpperPageSize : pageSize;

        var totalCount = await query.CountAsync();
        var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

        return new(items, pageIndex, pageSize, totalCount);
    }
}