namespace SilentMike.DietMenu.Shared.Core.Interfaces;

public interface ICoreDataMessage
{
    string Payload { get; }
    string PayloadType { get; }
}
