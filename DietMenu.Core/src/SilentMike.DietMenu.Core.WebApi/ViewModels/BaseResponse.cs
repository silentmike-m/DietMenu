namespace SilentMike.DietMenu.Core.WebApi.ViewModels;

internal sealed class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "OK";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;
    [JsonPropertyName("type")] public string? ResponseType { get; set; } = default;
}
