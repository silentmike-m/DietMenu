namespace SilentMike.DietMenu.Mailing.Application.Emails.ViewModels;

public sealed record EmailLinkedResource
{
    public string ContentId { get; init; } = string.Empty;
    public byte[] Data { get; init; } = Array.Empty<byte>();
    public string FileName { get; init; } = string.Empty;
}
