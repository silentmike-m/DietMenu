namespace SilentMike.DietMenu.Core.WebApi.Filters;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SilentMike.DietMenu.Core.Application.ViewModels;
using SilentMike.DietMenu.Core.WebApi.Extensions;
using SilentMike.DietMenu.Core.WebApi.Helpers;

[ExcludeFromCodeCoverage]
internal sealed class ApiActionFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        HandleResultsWithStatusCode(resultContext);
    }

    private static void HandleResultsWithStatusCode(ActionExecutedContext resultContext)
    {
        if (resultContext.Result is IStatusCodeActionResult statusCodeActionResult)
        {
            var statusCode = statusCodeActionResult.StatusCode ?? resultContext.HttpContext.Response.StatusCode;
            var baseResponse = resultContext.Result switch
            {
                ObjectResult objectResult => MapObjectResultToBaseResponse(objectResult, statusCode),
                _ => statusCode.MapToBaseResponse(),
            };
            resultContext.HttpContext.Response.Headers.Add(BaseResponseHelpers.RESPONSE_HAS_BEEN_HANDLED, nameof(ApiActionFilter));
            resultContext.Result = new ObjectResult(baseResponse)
            {
                ContentTypes = new MediaTypeCollection
                {
                    MediaTypeNames.Application.Json,
                },
                StatusCode = 200,
            };
        }
    }

    private static BaseResponse<object> MapObjectResultToBaseResponse(ObjectResult result, int statusCode)
    {
        if (result is { Value: { } } response)
        {
            var responseType = response.Value.GetType();
            return statusCode.MapToBaseResponse(response.Value, responseType);
        }

        return statusCode.MapToBaseResponse();
    }
}
