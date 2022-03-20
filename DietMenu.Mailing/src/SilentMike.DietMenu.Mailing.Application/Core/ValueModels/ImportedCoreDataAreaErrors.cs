namespace SilentMike.DietMenu.Mailing.Application.Core.ValueModels;

using System.Text.Json.Serialization;

public sealed record ImportedCoreDataAreaErrors
{
    [JsonPropertyName("data_area")] public string DataArea { get; init; } = string.Empty;
    [JsonPropertyName("errors")] public List<ImportedCoreDataError> Errors { get; init; } = new();
}
