namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class CreatedFamilyMessage : ICreatedFamilyMessage
{
    public Guid Id { get; init; } = Guid.Empty;
    public string Name { get; init; } = string.Empty;
}
