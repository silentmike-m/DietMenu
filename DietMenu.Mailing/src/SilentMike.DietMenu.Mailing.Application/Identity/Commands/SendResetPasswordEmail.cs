namespace SilentMike.DietMenu.Mailing.Application.Identity.Commands;

public sealed record SendResetPasswordEmail : IRequest
{
    public string Email { get; init; } = string.Empty;
    public string Url { get; init; } = string.Empty;
}
