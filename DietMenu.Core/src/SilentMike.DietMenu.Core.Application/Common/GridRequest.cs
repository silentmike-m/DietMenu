namespace SilentMike.DietMenu.Core.Application.Common;

public sealed record GridRequest
{
    [JsonPropertyName("filter")] public string Filter { get; init; } = string.Empty;
    [JsonPropertyName("is_descending")] public bool IsDescending { get; init; } = default;
    [JsonPropertyName("is_paged")] public bool IsPaged { get; init; } = default;
    [JsonPropertyName("order_by")] public string OrderBy { get; init; } = string.Empty;
    [JsonPropertyName("page_number")] public int PageNumber { get; init; } = default;
    [JsonPropertyName("page_size")] public int PageSize { get; init; } = default;
}
