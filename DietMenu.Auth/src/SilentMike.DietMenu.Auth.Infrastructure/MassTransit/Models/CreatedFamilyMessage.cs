namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Core;

internal sealed record CreatedFamilyMessage : ICreatedFamilyMessage
{
    public Guid Id { get; init; } = Guid.Empty;
}
