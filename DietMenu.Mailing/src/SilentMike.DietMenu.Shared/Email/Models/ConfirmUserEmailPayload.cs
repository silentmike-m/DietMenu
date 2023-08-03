namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ConfirmUserEmailPayload
{
    public string Email { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
