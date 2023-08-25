namespace SilentMike.DietMenu.Core.Application.Families.Events;

using SilentMike.DietMenu.Core.Application.Families.Models;

public sealed record ImportedFamilyData : INotification
{
    public string? ErrorCode { get; init; } = default;
    public string? ErrorMessage { get; init; } = default;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public IReadOnlyList<ImportFamilyDataResult> Results { get; init; } = new List<ImportFamilyDataResult>();
}
