namespace SilentMike.DietMenu.Shared.Email.Interfaces;

public interface IEmailDataMessage
{
    string Payload { get; }
    string PayloadType { get; }
}
