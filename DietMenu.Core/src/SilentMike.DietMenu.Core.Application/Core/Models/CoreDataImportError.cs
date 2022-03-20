namespace SilentMike.DietMenu.Core.Application.Core.Models;

public sealed record CoreDataImportError
{
    public string DataArea { get; init; } = string.Empty;
    public IDictionary<string, string[]> Errors { get; init; } = new Dictionary<string, string[]>();
}
