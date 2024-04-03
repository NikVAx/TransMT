namespace TrackMS.Domain.ServiceResultAPI;

public class ServiceResult
{
    private readonly List<ErrorMessage> _errors = [];

    public bool Succeeded { get; set; }

    public IEnumerable<ErrorMessage> Errors 
    { 
        get => _errors; 
        init => _errors = value.ToList(); 
    }

    public void AddError(int code, string message)
    {   
        _errors.Add(new ErrorMessage(code, message));
    }
}

public class ObjectServiceResult<TObject> : ServiceResult
{
    public TObject Object { get; set; }
}

public class ServiceResults
{
    public static ServiceResult Success()
    { 
        return new ServiceResult
        { 
            Succeeded = true 
        };
    }

    public static ObjectServiceResult<TObject> Success<TObject>(TObject @object)
    {
        return new ObjectServiceResult<TObject>
        {
            Object = @object,
            Succeeded = true
        };
    }

    public static ServiceResult Fail(params ErrorMessage[] errors)
    {
        return new ServiceResult
        {
            Succeeded = false,
            Errors = errors
        };
    }

    public static ObjectServiceResult<TObject> Fail<TObject>(params ErrorMessage[] errors)
    {
        return new ObjectServiceResult<TObject>
        {
            Succeeded = false,
            Errors = errors,
            Object = default!
        };
    }
}