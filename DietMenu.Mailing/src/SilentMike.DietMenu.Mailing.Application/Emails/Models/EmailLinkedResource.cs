namespace SilentMike.DietMenu.Mailing.Application.Emails.Models;

public sealed record EmailLinkedResource(string ContentId, byte[] Data, string FileName);
