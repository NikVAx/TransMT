namespace TrackMS.Domain.ServiceResultAPI;

public class ErrorMessage(int code, string message)
{
    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
}