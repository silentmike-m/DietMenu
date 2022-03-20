namespace SilentMike.DietMenu.Mailing.Application.Family.ValueModels;

using System.Text.Json.Serialization;

public sealed record ImportedFamilyDataAreaErrors
{
    [JsonPropertyName("data_area")] public string DataArea { get; init; } = string.Empty;
    [JsonPropertyName("errors")] public List<ImportedFamilyDataError> Errors { get; init; } = new();
}
