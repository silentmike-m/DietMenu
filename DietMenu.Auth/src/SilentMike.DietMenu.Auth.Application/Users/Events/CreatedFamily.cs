namespace SilentMike.DietMenu.Auth.Application.Users.Events;

public sealed record CreatedFamily : INotification
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
}
