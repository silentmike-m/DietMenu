﻿namespace SilentMike.DietMenu.Core.WebApi.Extensions;

using SilentMike.DietMenu.Core.Application.ViewModels;

internal static class BaseResponseExtensions
{
    public static BaseResponse<object> MapToBaseResponse(this int statusCode, object? response = null, Type? responseType = null)
    {
        return statusCode switch
        {
            200 => new BaseResponse<object>
            {
                Code = "ok",
                Response = response,
                ResponseType = responseType?.FullName
                               ?? typeof(OkBaseResponse).FullName,
            },
            204 => new BaseResponse<object>
            {
                Code = "ok",
                Response = response,
                ResponseType = responseType?.FullName
                               ?? typeof(OkBaseResponse).FullName,
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
            403 => new BaseResponse<object>
            {
                Code = "forbidden",
                Error = "Request has not been properly authorized",
            },
            404 => new BaseResponse<object>
            {
                Code = "not_found",
                Error = "Requested resource has not been found",
            },
            405 => new BaseResponse<object>
            {
                Code = "method_not_allowed",
                Error = "Requested method is not allowed",
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
}
