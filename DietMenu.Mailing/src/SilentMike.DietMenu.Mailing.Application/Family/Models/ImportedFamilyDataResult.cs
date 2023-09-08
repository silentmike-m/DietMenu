namespace SilentMike.DietMenu.Mailing.Application.Family.Models;

public sealed record ImportedFamilyDataResult
{
    public string DataArea { get; init; } = string.Empty;
    public List<ImportedFamilyDataError> Errors { get; init; } = new();
}
