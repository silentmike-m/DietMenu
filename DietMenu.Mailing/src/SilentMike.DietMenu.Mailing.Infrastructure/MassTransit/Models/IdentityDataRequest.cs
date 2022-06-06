namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.Identity.Interfaces;

internal sealed record IdentityDataRequest : IIdentityDataRequest
{
    public string Payload { get; init; } = string.Empty;
    public string PayloadType { get; init; } = string.Empty;
}
