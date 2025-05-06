using Microsoft.EntityFrameworkCore;

namespace BookAPI.DTOs.ResponseDTOs;

public class PagedList<T>
{
    private PagedList(List<T> data, int pageIndex, int pageSize, int itemCount)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        ItemCount = itemCount;
    }

    public List<T> Data { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int ItemCount { get; set; }
    public bool HasNext => (PageIndex * PageSize) + PageSize < ItemCount;
    public bool HasPrevious => PageIndex > 0;

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageIndex, int pageSize, CancellationToken ct)
    {
        var count = await query.CountAsync(ct);
        var data = await query.Skip(pageSize * pageIndex).Take(pageSize).ToListAsync(ct);

        return new PagedList<T>(
            data: data,
            pageIndex: pageIndex,
            pageSize: pageSize,
            itemCount: count);
    }
}
