namespace SilentMike.DietMenu.Shared.MassTransit;

public interface ISendResetPasswordMessage
{
    string Email { get; }
    string Url { get; }
}
