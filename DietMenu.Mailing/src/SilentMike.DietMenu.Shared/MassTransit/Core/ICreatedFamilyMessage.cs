namespace SilentMike.DietMenu.Shared.MassTransit.Core;

public interface ICreatedFamilyMessage
{
    Guid Id { get; }
    string Name { get; }
}
