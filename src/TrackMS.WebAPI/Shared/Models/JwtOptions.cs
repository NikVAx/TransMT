namespace TrackMS.WebAPI.Shared.Models;

public class JwtOptions
{
    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int LifetimeInSeconds { get; set; }
}
