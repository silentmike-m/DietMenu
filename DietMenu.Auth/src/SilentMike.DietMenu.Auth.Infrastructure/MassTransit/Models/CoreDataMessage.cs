namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Core.Interfaces;

internal sealed record CoreDataMessage : ICoreDataMessage
{
    public string Payload { get; init; } = string.Empty;
    public string PayloadType { get; init; } = string.Empty;
}
