namespace TrackMS.WebAPI.Shared.DTO;

public class PageResponseDto<TEntityDto>(IEnumerable<TEntityDto> items, int pageSize, int pageIndex, int totalCount)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalCount;
    public int PageCount { get => TotalCount / PageSize; }

    public  IEnumerable<TEntityDto> Items { get; set; } = items;
}
