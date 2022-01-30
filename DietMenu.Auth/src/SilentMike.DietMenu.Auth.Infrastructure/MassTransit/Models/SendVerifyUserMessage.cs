namespace SilentMike.DietMenu.Auth.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendVerifyUserMessage : ISendVerifyUserMessage
{
    public string Email { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
