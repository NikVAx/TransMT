namespace TrackMS.WebAPI.Shared.DTO;

public class ErrorResponse
{
    public IEnumerable<ErrorMessage> Errors { get; set; } = [];   
}
