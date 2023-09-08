namespace SilentMike.DietMenu.Mailing.Application.Emails.Commands;

using SilentMike.DietMenu.Mailing.Application.Emails.Models;

public sealed record SendEmail(Email Email) : IRequest;
