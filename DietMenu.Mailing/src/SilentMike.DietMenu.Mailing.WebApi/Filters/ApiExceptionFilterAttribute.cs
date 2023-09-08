namespace SilentMike.DietMenu.Mailing.WebApi.Filters;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using SilentMike.DietMenu.Mailing.Application.Common;
using SilentMike.DietMenu.Mailing.Application.Exceptions;
using SilentMike.DietMenu.Mailing.WebApi.Models.ViewModels;

[ExcludeFromCodeCoverage]
internal sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<ApiExceptionFilterAttribute> logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        => this.logger = logger;

    public override void OnException(ExceptionContext context)
    {
        this.HandleException(context);

        base.OnException(context);
    }

    private void HandleApplicationException(ExceptionContext context)
    {
        if (context.Exception is not ApplicationException exception)
        {
            throw new UnhandledErrorException();
        }

        var response = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
        };

        context.Result = new ObjectResult(response)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = StatusCodes.Status200OK,
        };

        context.ExceptionHandled = true;
    }

    private void HandleException(ExceptionContext context)
    {
        this.logger.LogError(context.Exception, "{Message}", context.Exception.Message);

        var exceptionHandler = context.Exception switch
        {
            ValidationException => this.HandleValidationException,
            ApplicationException => this.HandleApplicationException,
            _ => new Action<ExceptionContext>(this.HandleUnknownException),
        };

        exceptionHandler.Invoke(context);
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var response = new BaseResponse<object>
        {
            Code = ErrorCodes.UNKNOWN_ERROR,
            Error = ErrorCodes.UNKNOWN_ERROR_MESSAGE,
        };

        context.Result = new ObjectResult(response)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException exception)
        {
            throw new UnhandledErrorException();
        }

        var response = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
            Response = exception.Errors,
        };

        context.Result = new ObjectResult(response)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = StatusCodes.Status500InternalServerError,
        };

        context.ExceptionHandled = true;
    }
}
