namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ImportFamilyDataResult
{
    public string DataArea { get; init; } = string.Empty;
    public IReadOnlyList<ImportFamilyDataError> Errors { get; set; } = new List<ImportFamilyDataError>();
}
