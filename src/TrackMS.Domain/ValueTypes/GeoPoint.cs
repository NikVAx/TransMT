namespace TrackMS.Domain.ValueTypes;

public class GeoPoint(double lat, double lng)
{
    public double Lat { get; set; } = lat;
    public double Lng { get; set; } = lng;
}
