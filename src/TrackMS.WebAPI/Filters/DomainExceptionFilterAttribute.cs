using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackMS.Domain.Exceptions;

namespace TrackMS.WebAPI.Filters;

public class DomainExceptionFilterAttribute 
    : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch(context.Exception)
        {
            case NotFoundException exception:
                context.Result = new NotFoundObjectResult(
                new 
                { 
                    Error = exception.Message,
                });
                context.ExceptionHandled = true;
                break;
                    
        }
    }
}
