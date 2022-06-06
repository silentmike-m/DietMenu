namespace SilentMike.DietMenu.Core.Application.Core.Commands;

public sealed record ImportCoreData : IRequest
{
    [JsonPropertyName("validation_only")] public bool ValidationOnly { get; init; } = default;
}
