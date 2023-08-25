namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ImportedFamilyDataPayload
{
    public string? ErrorCode { get; init; } = default;
    public string? ErrorMessage { get; init; } = default;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public IReadOnlyList<ImportFamilyDataResult> Results { get; init; } = new List<ImportFamilyDataResult>();
}
