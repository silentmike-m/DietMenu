namespace DietMenu.Api.Application.Common;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

[ExcludeFromCodeCoverage]
public sealed class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "OK";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; init; } = default;
}
