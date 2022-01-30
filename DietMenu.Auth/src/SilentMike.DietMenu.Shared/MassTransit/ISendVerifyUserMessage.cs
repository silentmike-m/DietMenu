namespace SilentMike.DietMenu.Shared.MassTransit;

public interface ISendVerifyUserMessage
{
    string Email { get; }
    string Url { get; }
}
