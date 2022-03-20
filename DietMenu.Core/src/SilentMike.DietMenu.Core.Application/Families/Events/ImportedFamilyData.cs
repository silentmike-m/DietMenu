namespace SilentMike.DietMenu.Core.Application.Families.Events;

using SilentMike.DietMenu.Core.Application.Families.Models;

public sealed record ImportedFamilyData : INotification
{
    public IReadOnlyList<FamilyDataImportError> Errors { get; init; } = new List<FamilyDataImportError>();
    public Guid FamilyId { get; init; } = Guid.Empty;
    public bool IsSuccess { get; init; } = default;
}
