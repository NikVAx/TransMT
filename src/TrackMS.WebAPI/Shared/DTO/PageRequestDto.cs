using TrackMS.Domain.Enums;
using TrackMS.Domain.Interfaces;

namespace TrackMS.WebAPI.Shared.DTO;

public class PageRequestDto : ISortOptions
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
}