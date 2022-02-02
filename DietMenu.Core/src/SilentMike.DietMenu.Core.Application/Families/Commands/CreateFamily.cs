namespace SilentMike.DietMenu.Core.Application.Families.Commands;

using System.Text.Json.Serialization;
using MediatR;

public sealed record CreateFamily : IRequest
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
}
