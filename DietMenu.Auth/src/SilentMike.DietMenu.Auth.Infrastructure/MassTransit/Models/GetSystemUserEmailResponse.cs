namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed record GetSystemUserEmailResponse : IGetSystemUserEmailResponse
{
    public string Email { get; init; } = string.Empty;
}
