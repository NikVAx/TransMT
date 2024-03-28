using TrackMS.Domain.Enums;

namespace TrackMS.Domain.Interfaces;

public interface ISortOptions
{
    public string? SortBy { get; }
    public SortOrder SortOrder { get; }
}
