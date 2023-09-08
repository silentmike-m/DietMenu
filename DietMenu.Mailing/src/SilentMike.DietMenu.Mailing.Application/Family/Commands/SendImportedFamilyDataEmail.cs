namespace SilentMike.DietMenu.Mailing.Application.Family.Commands;

using SilentMike.DietMenu.Mailing.Application.Family.Models;

public sealed record SendImportedFamilyDataEmail : IRequest
{
    public string? ErrorCode { get; init; } = default;
    public string? ErrorMessage { get; init; } = default;
    public Guid FamilyId { get; init; } = Guid.Empty;
    public bool IsSuccess { get; init; } = default;
    public List<ImportedFamilyDataResult> Results { get; init; } = new();
}
