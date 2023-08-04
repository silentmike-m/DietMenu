namespace SilentMike.DietMenu.Core.Application.Families.Commands;

public sealed record CreateFamily : IRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
}
