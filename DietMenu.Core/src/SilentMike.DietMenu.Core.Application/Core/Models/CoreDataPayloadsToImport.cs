namespace SilentMike.DietMenu.Core.Application.Core.Models;

public sealed record CoreDataPayloadsToImport
{
    public byte[] IngredientsPayload { get; init; } = Array.Empty<byte>();
}
