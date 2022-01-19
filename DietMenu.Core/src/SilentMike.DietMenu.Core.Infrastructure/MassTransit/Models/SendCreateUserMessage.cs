namespace SilentMike.DietMenu.Core.Infrastructure.MassTransit.Models;

using SilentMike.DietMenu.Shared.MassTransit;

internal sealed class SendCreateUserMessage : ISendCreatedUserMessage
{
    public string Email { get; set; } = string.Empty;
    public string FamilyName { get; set; } = string.Empty;
    public string LoginUrl { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
}
