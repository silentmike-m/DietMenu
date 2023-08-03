namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Core.Models;

using SilentMike.DietMenu.Shared.Core.Interfaces;

internal sealed record CreatedFamilyMessage : ICreatedFamilyMessage
{
    public Guid Id { get; init; } = Guid.Empty;
}
