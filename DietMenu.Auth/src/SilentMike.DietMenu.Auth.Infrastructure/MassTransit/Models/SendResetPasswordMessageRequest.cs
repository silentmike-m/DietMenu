namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;
using SilentMike.DietMenu.Shared.MassTransit.Identity;

internal sealed record SendResetPasswordMessageRequest : ISendResetPasswordMessageRequest
{
    public string Email { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
