namespace SilentMike.DietMenu.Core.Application.Core.Queries;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Core.Models;

public sealed record GetCoreDataToImport : IRequest<CoreDataToImport>, IGetCoreDataToImport
{
    [JsonPropertyName("ingredients_payload")] public byte[] IngredientsPayload { get; init; } = Array.Empty<byte>();
}
