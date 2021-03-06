namespace SilentMike.DietMenu.Auth.Web.Filters;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions;
using ApplicationException = SilentMike.DietMenu.Auth.Application.Common.ApplicationException;

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

    private static void HandleApplicationException(ExceptionContext context)
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
            StatusCode = 500,
        };
        context.ExceptionHandled = true;
    }

    private void HandleException(ExceptionContext context)
    {
        this.logger.LogError(context.Exception, "{Message}", context.Exception.Message);

        var exceptionHandler = context.Exception switch
        {
            ValidationException => HandleValidationException,
            ApplicationException => HandleApplicationException,
            _ => new Action<ExceptionContext>(HandleUnknownException),
        };

        exceptionHandler.Invoke(context);
    }

    private static void HandleUnknownException(ExceptionContext context)
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
            StatusCode = 500,
        };
        context.ExceptionHandled = true;
    }

    private static void HandleValidationException(ExceptionContext context)
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
            StatusCode = 500,
        };
        context.ExceptionHandled = true;
    }
}
