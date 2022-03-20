namespace SilentMike.DietMenu.Mailing.Application.Core.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Mailing.Application.Core.ValueModels;

public sealed record SendImportedCoreDataEmail : IRequest
{
    [JsonPropertyName("data_errors")] public List<ImportedCoreDataAreaErrors> DataErrors { get; init; } = new();
    [JsonPropertyName("is_success")] public bool IsSuccess { get; init; } = default;
    [JsonPropertyName("server")] public string Server { get; init; } = string.Empty;
}
