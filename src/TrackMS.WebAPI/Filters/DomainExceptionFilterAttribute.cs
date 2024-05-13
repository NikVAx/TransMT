using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TrackMS.Domain.Exceptions;
using TrackMS.WebAPI.Shared.DTO;

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

            case ConflictException exception:
                context.Result = new ConflictObjectResult(
                    new ErrorResponse
                    {
                       Errors = 
                       [ 
                           new ErrorMessage("Conflict", exception.Message) 
                       ]
                    });
                context.ExceptionHandled = true;
                break;

            default:
                context.Result = new ObjectResult(context.Exception);
                break;

        }
    }
}
