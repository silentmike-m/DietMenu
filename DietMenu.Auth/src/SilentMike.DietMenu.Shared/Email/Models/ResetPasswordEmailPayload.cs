namespace SilentMike.DietMenu.Shared.Email.Models;

public sealed record ResetPasswordEmailPayload
{
    public string Email { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
