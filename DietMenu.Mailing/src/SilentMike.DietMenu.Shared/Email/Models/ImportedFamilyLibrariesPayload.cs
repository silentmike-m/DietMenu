namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed class ImportedFamilyLibrariesPayload
{
    public Dictionary<string, List<ImportedLibraryError>> AreaErrors { get; init; } = new();
    public Guid FamilyId { get; init; } = Guid.Empty;
}
