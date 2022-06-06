namespace SilentMike.DietMenu.Core.Application.ViewModels;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public sealed class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "OK";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;
    [JsonPropertyName("type")] public string? ResponseType { get; set; } = default;
}
