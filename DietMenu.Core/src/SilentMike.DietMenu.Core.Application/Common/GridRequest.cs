namespace SilentMike.DietMenu.Core.Application.Common;

public sealed record GridRequest
{
    public string Filter { get; init; } = string.Empty;
    public bool IsDescending { get; init; } = default;
    public bool IsPaged { get; init; } = default;
    public string OrderBy { get; init; } = string.Empty;
    public int PageNumber { get; init; } = default;
    public int PageSize { get; init; } = default;
}
