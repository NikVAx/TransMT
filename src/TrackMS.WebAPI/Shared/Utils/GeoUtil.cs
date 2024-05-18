using NetTopologySuite.Geometries;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Shared.Utils;

public static class GeoUtil
{
    public static Polygon CreatePolygon(IEnumerable<GeoPoint> points)
    {
        var coordinates = points
            .Select(x => new Coordinate(x.Lat, x.Lng))
            .Append(new Coordinate(points.First().Lat, points.First().Lng))
            .ToArray();

        return Geometry.DefaultFactory.CreatePolygon(coordinates);
    }
}
