namespace SilentMike.DietMenu.Mailing.Application.Emails.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Mailing.Application.Common;

public sealed record SendEmail : IRequest
{
    [JsonPropertyName("email")] public Email Email { get; init; } = new();
}
