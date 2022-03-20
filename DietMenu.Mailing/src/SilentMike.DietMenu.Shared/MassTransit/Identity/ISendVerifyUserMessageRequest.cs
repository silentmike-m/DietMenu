namespace SilentMike.DietMenu.Shared.MassTransit.Identity;

public interface ISendVerifyUserMessageRequest
{
    string Email { get; }
    string Url { get; }
}
