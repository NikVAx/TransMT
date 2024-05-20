using NetTopologySuite.Geometries;
using TrackMS.Domain.ValueTypes;

namespace TrackMS.WebAPI.Shared.Utils;

public static class GeoUtil
{
    public static Polygon CreatePolygon(IEnumerable<GeoPoint> points)
    {
        var coordinates = points
            .Select(x => new Coordinate(x.Lng, x.Lat))
            .Append(new Coordinate(points.First().Lng, points.First().Lat))
            .ToArray();

        return Geometry.DefaultFactory.CreatePolygon(coordinates);
    }
}
