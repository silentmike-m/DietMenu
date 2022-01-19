namespace SilentMike.DietMenu.Shared.MassTransit;

public interface ISendVerifyEmailRequest
{
    string Email { get; }
    string Url { get; }
    string UserName { get; }
}
