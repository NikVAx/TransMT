namespace TrackMS.Domain.ValueTypes;

public class GeoPoint(double latitute, double longitude)
{
    public double Latitute { get; set; } = latitute;
    public double Longitude { get; set; } = longitude;
}
