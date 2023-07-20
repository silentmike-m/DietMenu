namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ImportedLibraryError
{
    public string Code { get; init; } = string.Empty;
    public List<string> Messages { get; init; } = new();
}
