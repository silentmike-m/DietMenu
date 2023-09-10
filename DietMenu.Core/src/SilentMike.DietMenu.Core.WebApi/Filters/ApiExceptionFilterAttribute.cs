namespace SilentMike.DietMenu.Core.WebApi.Filters;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Domain.Common;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.WebApi.ViewModels;

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

    private void HandleException(ExceptionContext context)
    {
        this.logger.LogError(context.Exception, "{Message}", context.Exception.Message);

        var exceptionHandler = context.Exception switch
        {
            ValidationException => HandleValidationException,
            ApplicationException => HandleApplicationException,
            DomainException => HandleDomainException,
            InfrastructureException => HandleInfrastructureException,
            _ => new Action<ExceptionContext>(HandleUnknownException),
        };

        exceptionHandler.Invoke(context);
    }

    private static void FillContextResult(ExceptionContext context, string code, string message, object? response = null)
    {
        var resultResponse = new BaseResponse<object>
        {
            Code = code,
            Error = message,
            Response = response,
        };

        context.Result = new ObjectResult(resultResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };
    }

    private static void HandleApplicationException(ExceptionContext context)
    {
        if (context.Exception is not ApplicationException exception)
        {
            throw new UnhandledErrorException();
        }

        FillContextResult(context, exception.Code, exception.Message);

        context.ExceptionHandled = true;
    }

    private static void HandleDomainException(ExceptionContext context)
    {
        if (context.Exception is not DomainException exception)
        {
            throw new UnhandledErrorException();
        }

        FillContextResult(context, exception.Code, exception.Message);

        context.ExceptionHandled = true;
    }

    private static void HandleInfrastructureException(ExceptionContext context)
    {
        if (context.Exception is not InfrastructureException exception)
        {
            throw new UnhandledErrorException();
        }

        FillContextResult(context, exception.Code, exception.Message);

        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        FillContextResult(context, ErrorCodes.UNKNOWN_ERROR, ErrorCodes.UNKNOWN_ERROR_MESSAGE);

        context.ExceptionHandled = true;
    }

    private static void HandleValidationException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException exception)
        {
            throw new UnhandledErrorException();
        }

        FillContextResult(context, exception.Code, exception.Message, exception.Errors);

        context.ExceptionHandled = true;
    }
}
