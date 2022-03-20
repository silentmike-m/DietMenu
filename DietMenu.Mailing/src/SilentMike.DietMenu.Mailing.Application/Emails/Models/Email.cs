namespace SilentMike.DietMenu.Mailing.Application.Emails.Models;

public sealed record Email
{
    public string HtmlMessage { get; init; } = string.Empty;

    public IReadOnlyList<EmailLinkedResource> LinkedResources { get; init; } = new List<EmailLinkedResource>().AsReadOnly();
    public string Receiver { get; init; } = string.Empty;
    public string Subject { get; init; } = string.Empty;
    public string TextMessage { get; init; } = string.Empty;
}
