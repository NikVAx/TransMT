using System.Text.Json.Serialization;

namespace TrackMS.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortOrder
{
    Ascending,
    Descending
}
