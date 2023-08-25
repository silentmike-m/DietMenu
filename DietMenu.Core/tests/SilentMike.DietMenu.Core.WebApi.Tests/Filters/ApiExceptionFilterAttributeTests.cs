namespace SilentMike.DietMenu.Core.WebApi.Tests.Filters;

using System.Net.Mime;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.WebApi.Filters;
using SilentMike.DietMenu.Core.WebApi.ViewModels;

[TestClass]
public sealed class ApiExceptionFilterAttributeTests
{
    private readonly NullLogger<ApiExceptionFilterAttribute> logger = new();

    [TestMethod]
    public void Should_Handle_Application_Exception()
    {
        //GIVEN
        var exception = new DietMenuUnauthorizedException(Guid.NewGuid(), Guid.NewGuid());

        var actionContext = new ActionContext
        {
            ActionDescriptor = new ActionDescriptor(),
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception,
        };

        var filter = new ApiExceptionFilterAttribute(this.logger);

        //WHEN
        filter.OnException(context);

        //THEN
        var expectedResponse = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
        };

        var expectedResult = new ObjectResult(expectedResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };

        context.Result.Should()
            .BeEquivalentTo(expectedResult)
            ;

        context.ExceptionHandled.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_Handle_Domain_Exception()
    {
        //GIVEN
        var exception = new IngredientEmptyNameException(Guid.NewGuid());

        var actionContext = new ActionContext
        {
            ActionDescriptor = new ActionDescriptor(),
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception,
        };

        var filter = new ApiExceptionFilterAttribute(this.logger);

        //WHEN
        filter.OnException(context);

        //THEN
        var expectedResponse = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
        };

        var expectedResult = new ObjectResult(expectedResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };

        context.Result.Should()
            .BeEquivalentTo(expectedResult)
            ;

        context.ExceptionHandled.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_Handle_Exception()
    {
        //GIVEN
        var exception = new ArgumentException();

        var actionContext = new ActionContext
        {
            ActionDescriptor = new ActionDescriptor(),
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception,
        };

        var filter = new ApiExceptionFilterAttribute(this.logger);

        //WHEN
        filter.OnException(context);

        //THEN
        var expectedResponse = new BaseResponse<object>
        {
            Code = ErrorCodes.UNKNOWN_ERROR,
            Error = ErrorCodes.UNKNOWN_ERROR_MESSAGE,
        };

        var expectedResult = new ObjectResult(expectedResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };

        context.Result.Should()
            .BeEquivalentTo(expectedResult)
            ;

        context.ExceptionHandled.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_Handle_Infrastructure_Exception()
    {
        //GIVEN
        var exception = new IngredientEmptyNameException(Guid.NewGuid());

        var actionContext = new ActionContext
        {
            ActionDescriptor = new ActionDescriptor(),
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception,
        };

        var filter = new ApiExceptionFilterAttribute(this.logger);

        //WHEN
        filter.OnException(context);

        //THEN
        var expectedResponse = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
        };

        var expectedResult = new ObjectResult(expectedResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };

        context.Result.Should()
            .BeEquivalentTo(expectedResult)
            ;

        context.ExceptionHandled.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_Handle_Validation_Exception()
    {
        //GIVEN
        var failures = new ValidationFailure("exchanger", "error message")
        {
            ErrorCode = "error code",
        };

        var exception = new ValidationException(new[] { failures });

        var actionContext = new ActionContext
        {
            ActionDescriptor = new ActionDescriptor(),
            HttpContext = new DefaultHttpContext(),
            RouteData = new RouteData(),
        };

        var context = new ExceptionContext(actionContext, new List<IFilterMetadata>())
        {
            Exception = exception,
        };

        var filter = new ApiExceptionFilterAttribute(this.logger);

        //WHEN
        filter.OnException(context);

        //THEN
        var expectedResponse = new BaseResponse<object>
        {
            Code = exception.Code,
            Error = exception.Message,
            Response = exception.Errors,
        };

        var expectedResult = new ObjectResult(expectedResponse)
        {
            ContentTypes = new MediaTypeCollection
            {
                MediaTypeNames.Application.Json,
            },
            StatusCode = 500,
        };

        context.Result.Should()
            .BeEquivalentTo(expectedResult)
            ;

        context.ExceptionHandled.Should()
            .BeTrue()
            ;
    }
}
