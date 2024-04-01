namespace TrackMS.WebAPI.Shared.DTO;

public class PageResponseDto<TEntityDto>
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; set; }
    public int PageCount { get => TotalCount / PageSize; }

    public required IEnumerable<TEntityDto> Items { get; set; }
}
