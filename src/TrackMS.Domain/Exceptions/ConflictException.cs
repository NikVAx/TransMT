namespace TrackMS.Domain.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string message = "Unique keys conflict", Exception? innerException = null)
        : base(message, innerException)
    {

    }
}
