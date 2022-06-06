namespace SilentMike.DietMenu.Core.Application.Families.Commands;

public sealed record CreateFamily : IRequest
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
}
