namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendVerifyEmailRequest : ISendVerifyEmailRequest
{
    public string Email { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
