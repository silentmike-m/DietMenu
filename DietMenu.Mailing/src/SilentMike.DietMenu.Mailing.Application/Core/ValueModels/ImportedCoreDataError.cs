namespace SilentMike.DietMenu.Mailing.Application.Core.ValueModels;

using System.Text.Json.Serialization;

public sealed record ImportedCoreDataError
{
    [JsonPropertyName("code")] public string Code { get; init; } = string.Empty;
    [JsonPropertyName("messages")] public List<string> Messages { get; init; } = new();
}
