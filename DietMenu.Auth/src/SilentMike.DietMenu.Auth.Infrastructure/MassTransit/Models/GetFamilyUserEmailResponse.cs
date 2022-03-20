namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed record GetFamilyUserEmailResponse : IGetFamilyUserEmailResponse
{
    public string Email { get; init; } = string.Empty;
}
