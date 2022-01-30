namespace SilentMike.DietMenu.Auth.Application.Common;

public class BaseResponse<T>
{
    [JsonPropertyName("code")] public string Code { get; init; } = "ok";
    [JsonPropertyName("error")] public string? Error { get; init; } = default;
    [JsonPropertyName("response")] public T? Response { get; set; } = default;
}
