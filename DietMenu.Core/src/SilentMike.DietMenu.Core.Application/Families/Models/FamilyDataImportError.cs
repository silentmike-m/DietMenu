namespace SilentMike.DietMenu.Core.Application.Families.Models;

public sealed record FamilyDataImportError
{
    public string DataArea { get; init; } = string.Empty;
    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();
}
