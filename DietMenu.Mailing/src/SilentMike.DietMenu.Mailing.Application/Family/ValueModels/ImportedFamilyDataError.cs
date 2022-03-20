namespace SilentMike.DietMenu.Mailing.Application.Family.ValueModels;

using System.Text.Json.Serialization;

public sealed record ImportedFamilyDataError
{
    [JsonPropertyName("code")] public string Code { get; init; } = string.Empty;
    [JsonPropertyName("messages")] public List<string> Messages { get; init; } = new();
}
