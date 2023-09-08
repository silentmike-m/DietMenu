﻿namespace SilentMike.DietMenu.Mailing.WebApi.Models.ViewModels;

using System.Text.Json.Serialization;

internal sealed class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "ok";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;
    [JsonPropertyName("type")] public string? ResponseType { get; set; } = default;
}
