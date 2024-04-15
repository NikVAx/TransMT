namespace TrackMS.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Exception? innerException = null) 
        : base("Entity Not Found", innerException)
    {

    }
}
