// ReSharper disable ConstantConditionalAccessQualifier
namespace SilentMike.DietMenu.Core.WebApi.Middlewares;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.IO;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.WebApi.Extensions;

[ExcludeFromCodeCoverage]
internal sealed class KestrelResponseHandlerMiddleware
{
    private const string APPLICATION_JSON = "application/json; charset=utf-8";
    private const string OK_STATUS_CODE = "OK";

    private readonly RequestDelegate next;

    public KestrelResponseHandlerMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var originalResponseBodyStream = httpContext.Response.Body;

        var recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        await using var responseStream = recyclableMemoryStreamManager.GetStream();
        httpContext.Response.Body = responseStream;

        await next(httpContext);

        var isRedirect = httpContext.Response.StatusCode == 302;

        if (isRedirect)
        {
            return;
        }

        responseStream.Position = 0;
        using var currentResponseStreamReader = new StreamReader(responseStream);

        var currentResponse = await currentResponseStreamReader.ReadToEndAsync();

        var newResponseObject = CreateNewResponse(httpContext.Response.StatusCode);

        var isResponseContentTypeJson = httpContext.Response.ContentType?
            .Contains("json", StringComparison.InvariantCultureIgnoreCase) ?? false;

        var isResponseContentTypeTextPlain = httpContext.Response.ContentType?
            .Contains(MediaTypeNames.Text.Plain, StringComparison.InvariantCultureIgnoreCase) ?? false;

        var isResponseContentTypeOctetStream = httpContext.Response.ContentType?
            .Contains(MediaTypeNames.Application.Octet, StringComparison.InvariantCultureIgnoreCase) ?? false;

        var isSwaggerPage = httpContext.Request.Path.Value?
            .Contains("swagger", StringComparison.InvariantCultureIgnoreCase) ?? false;

        var isSwitchingProtocolsRequest = httpContext.Response.StatusCode == 101;

        if (isSwitchingProtocolsRequest)
        {
            responseStream.Position = 0;
            await responseStream.CopyToAsync(originalResponseBodyStream);
            return;
        }

        var isResponseErrorDescription = false;

        if (isResponseContentTypeJson)
        {
            isResponseErrorDescription = IsResponseErrorDescription(currentResponse);
        }

        if (isResponseContentTypeJson
            && !string.IsNullOrWhiteSpace(currentResponse)
            && !isSwaggerPage
            && !isResponseErrorDescription)
        {
            newResponseObject.Response = currentResponse.ToObject<object>();
            await HandleJsonResponse(newResponseObject, httpContext, responseStream);
        }
        else if (isResponseContentTypeTextPlain
                 && !string.IsNullOrWhiteSpace(currentResponse))
        {
            newResponseObject.Response = currentResponse;
            await HandleJsonResponse(newResponseObject, httpContext, responseStream);
        }
        else if (httpContext.Response.ContentType is null)
        {
            if (!string.IsNullOrWhiteSpace(currentResponse))
            {
                newResponseObject.Response = currentResponse;
            }

            await HandleJsonResponse(newResponseObject, httpContext, responseStream);
        }
        else if (isResponseContentTypeOctetStream)
        {
            //ignore
        }
        else
        {
            await HandleOtherResponse(currentResponse, httpContext, responseStream);
        }

        HandleEveryResponse(httpContext);
        responseStream.Position = 0;
        await responseStream.CopyToAsync(originalResponseBodyStream);
    }

    private static BaseResponse<object> CreateNewResponse(int statusCode)
    {
        return statusCode switch
        {
            200 => new BaseResponse<object>
            {
                Code = OK_STATUS_CODE,
            },
            204 => new BaseResponse<object>
            {
                Code = OK_STATUS_CODE,
            },
            400 => new BaseResponse<object>
            {
                Code = "bad_request",
                Error = "Incorrect URL or content format",
            },
            401 => new BaseResponse<object>
            {
                Code = "unauthorized",
                Error = "Request has not been properly authorized",
            },
            404 => new BaseResponse<object>
            {
                Code = "not_found",
                Error = "Requested path has not been found",
            },
            500 => new BaseResponse<object>
            {
                Code = "internal_server_error",
                Error = "An unhandled exception was thrown by the application",
            },
            _ => new BaseResponse<object>
            {
                Code = statusCode.ToString(),
            },
        };
    }

    private static bool IsResponseErrorDescription(string response)
    {
        var jsonDocument = JsonDocument.Parse(response);
        var jsonRootElement = jsonDocument.RootElement;

        var codePropertyExists = jsonRootElement.TryGetProperty("code", out var codeJsonElement);

        if (!codePropertyExists)
        {
            return false;
        }

        if (codeJsonElement.ValueKind != JsonValueKind.String)
        {
            return false;
        }

        var codeValue = codeJsonElement.GetString();

        if (codeValue == OK_STATUS_CODE)
        {
            return false;
        }

        var errorPropertyExists = jsonRootElement.TryGetProperty("error", out var errorJsonElement);

        if (!errorPropertyExists)
        {
            return false;
        }

        if (errorJsonElement.ValueKind != JsonValueKind.String)
        {
            return false;
        }


        return true;
    }

    private static async Task HandleJsonResponse(BaseResponse<object>? response, HttpContext httpContext, MemoryStream responseStream)
    {
        if (response is null)
        {
            return;
        }

        var newResponseJsonString = response.ToIndentedIgnoreNullJson();
        var responseBuffer = Encoding.UTF8.GetBytes(newResponseJsonString);

        httpContext.Response.ContentType = APPLICATION_JSON;
        httpContext.Response.ContentLength = responseBuffer.LongLength;

        responseStream.Position = 0;
        await responseStream.WriteAsync(responseBuffer);
    }

    private static async Task HandleOtherResponse(string? response, HttpContext httpContext, MemoryStream responseStream)
    {
        if (response is null)
        {
            return;
        }

        var responseBuffer = Encoding.UTF8.GetBytes(response);

        httpContext.Response.ContentLength = responseBuffer.LongLength;

        responseStream.Position = 0;
        await responseStream.WriteAsync(responseBuffer);
    }

    private static void HandleEveryResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 200;
    }
}
