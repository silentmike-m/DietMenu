namespace SilentMike.DietMenu.Core.Infrastructure.Dapper.Models;

internal sealed record Filter
{
    public FilterJoinType FilterJoinType { get; init; } = FilterJoinType.And;
    public FilterType FilterType { get; init; } = FilterType.Equals;
    public bool IsString { get; init; } = default;
    public string PropertyName { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;

}
