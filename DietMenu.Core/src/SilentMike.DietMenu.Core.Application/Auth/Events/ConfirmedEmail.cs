namespace SilentMike.DietMenu.Core.Application.Auth.Events;

using System.Text.Json.Serialization;

public sealed record ConfirmedEmail : INotification
{
    [JsonPropertyName("email")] public string Email { get; init; } = string.Empty;
}
