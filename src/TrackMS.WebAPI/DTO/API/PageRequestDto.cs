﻿using TrackMS.Domain.Enums;
using TrackMS.Domain.Interfaces;

namespace TrackMS.WebAPI.DTO.API;

public class PageRequestDto : ISortOptions
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;

    public string? SortBy { get; set; } = null;
    public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
}