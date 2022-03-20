namespace SilentMike.DietMenu.Shared.MassTransit.Identity;

public interface ISendResetPasswordMessageRequest
{
    string Email { get; }
    string Url { get; }
}
