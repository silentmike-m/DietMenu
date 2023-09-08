namespace SilentMike.DietMenu.Mailing.Application.Emails.Models;

public sealed record Email
{
    public string HtmlMessage { get; init; }
    public IReadOnlyList<EmailLinkedResource> LinkedResources { get; init; }
    public string Receiver { get; init; }
    public string Subject { get; init; }
    public string TextMessage { get; init; }

    public Email(string htmlMessage, IReadOnlyList<EmailLinkedResource> linkedResources, string receiver, string subject, string textMessage)
    {
        this.HtmlMessage = htmlMessage;
        this.LinkedResources = linkedResources;
        this.Receiver = receiver;
        this.Subject = subject;
        this.TextMessage = textMessage;
    }
}
