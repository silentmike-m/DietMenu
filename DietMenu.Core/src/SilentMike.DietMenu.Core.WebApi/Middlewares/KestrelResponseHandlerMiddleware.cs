// ReSharper disable ConstantConditionalAccessQualifier
namespace SilentMike.DietMenu.Core.WebApi.Middlewares;

using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text;
using Microsoft.IO;
using SilentMike.DietMenu.Core.Application.ViewModels;
using SilentMike.DietMenu.Core.WebApi.Extensions;
using SilentMike.DietMenu.Core.WebApi.Helpers;

[ExcludeFromCodeCoverage]
internal sealed class KestrelResponseHandlerMiddleware
{
    private const string APPLICATION_JSON = "application/json; charset=utf-8";

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

        var shouldResponseBeForwarded = ShouldResponseBeForwarded(httpContext);

        if (shouldResponseBeForwarded)
        {
            responseStream.Position = 0;
            await responseStream.CopyToAsync(originalResponseBodyStream, CancellationToken.None);
            return;
        }

        responseStream.Position = 0;

        using var currentResponseStreamReader = new StreamReader(responseStream);

        var currentResponse = await currentResponseStreamReader.ReadToEndAsync();

        var hasResponseAlreadyBeenHandled = await HasResponseAlreadyBeenHandled(currentResponse, httpContext.Response.Headers, httpContext.Response.ContentType);

        if (hasResponseAlreadyBeenHandled)
        {
            httpContext.Response.Headers.Remove(BaseResponseHelpers.RESPONSE_HAS_BEEN_HANDLED);
            responseStream.Position = 0;
            await responseStream.CopyToAsync(originalResponseBodyStream, CancellationToken.None);
            return;
        }

        var formattedResponse = FormatResponse(currentResponse, httpContext.Response.ContentType);
        await HandleOtherResponseAsync(formattedResponse, httpContext, originalResponseBodyStream, responseStream, CancellationToken.None);
    }

    private static bool ShouldResponseBeForwarded(HttpContext httpContext)
    {
        var isRedirect = httpContext.Response.StatusCode == 302;

        if (isRedirect)
        {
            return true;
        }

        var isSwitchingProtocolsRequest = httpContext.Response.StatusCode == 101;

        if (isSwitchingProtocolsRequest)
        {
            return true;
        }

        var isSwaggerPage = httpContext.Request.Path.Value?
            .Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
        isSwaggerPage ??= false;

        if (isSwaggerPage.Value)
        {
            return true;
        }

        var isContentTypeFilled = (string?)httpContext.Response.ContentType is { };
        var isResponseContentTypeJson = httpContext.Response.ContentType?
            .Contains("json", StringComparison.InvariantCultureIgnoreCase);
        isResponseContentTypeJson ??= false;

        var isResponseContentTypeTextPlain = httpContext.Response.ContentType?
            .Contains(MediaTypeNames.Text.Plain, StringComparison.InvariantCultureIgnoreCase);
        isResponseContentTypeTextPlain ??= false;

        var isContentTypeSetAndItsNotJsonNorTextPlain = isContentTypeFilled
                                                        && !isResponseContentTypeJson.Value
                                                        && !isResponseContentTypeTextPlain.Value
                                                        ;

        if (isContentTypeSetAndItsNotJsonNorTextPlain)
        {
            return true;
        }

        return false;
    }

    private static Task<bool> HasResponseAlreadyBeenHandled(string? currentResponse, IHeaderDictionary headerDictionary, string? contentType)
    {
        var isResponseContentTypeJson = contentType?
            .Contains("json", StringComparison.InvariantCultureIgnoreCase);
        isResponseContentTypeJson ??= false;

        if (!isResponseContentTypeJson.Value)
        {
            return Task.FromResult(false);
        }

        var existResponseHasBeenHandledHeader = headerDictionary.ContainsKey(BaseResponseHelpers.RESPONSE_HAS_BEEN_HANDLED);

        BaseResponse<object>? baseResponse = null;
        var isDeserialized = currentResponse?.TryDeserialize(out baseResponse);
        isDeserialized ??= false;

        if (isDeserialized.Value)
        {
            var result = baseResponse is not null
                         && existResponseHasBeenHandledHeader
                ;
            return Task.FromResult(result);
        }

        return Task.FromResult(false);
    }

    private static object? FormatResponse(string currentResponse, string? contentType)
    {
        var isResponseContentTypeJson = contentType?
            .Contains("json", StringComparison.InvariantCultureIgnoreCase);
        isResponseContentTypeJson ??= false;

        if (isResponseContentTypeJson.Value && !string.IsNullOrWhiteSpace(currentResponse))
        {
            return currentResponse.ToObject<object>();
        }

        return currentResponse;
    }

    private static async Task HandleOtherResponseAsync(object? currentResponse, HttpContext httpContext, Stream originalResponseBodyStream, Stream responseStream, CancellationToken cancellationToken = default)
    {
        var baseResponse = httpContext.Response.StatusCode.MapToBaseResponse(currentResponse);
        await WriteBaseResponseToResponseStreamAsync(baseResponse, httpContext, responseStream, cancellationToken);
        httpContext.Response.StatusCode = 200;
        responseStream.Position = 0;
        await responseStream.CopyToAsync(originalResponseBodyStream, cancellationToken);
    }

    private static async Task WriteBaseResponseToResponseStreamAsync(BaseResponse<object>? baseResponse, HttpContext httpContext, Stream responseStream, CancellationToken cancellationToken = default)
    {
        if (baseResponse is null)
        {
            return;
        }

        var newResponseJsonString = baseResponse.ToIndentedIgnoreNullJson();
        var responseBuffer = Encoding.UTF8.GetBytes(newResponseJsonString);

        httpContext.Response.ContentType = APPLICATION_JSON;
        httpContext.Response.ContentLength = responseBuffer.LongLength;

        responseStream.Position = 0;
        responseStream.SetLength(0);
        await responseStream.WriteAsync(responseBuffer, cancellationToken);
    }
}
