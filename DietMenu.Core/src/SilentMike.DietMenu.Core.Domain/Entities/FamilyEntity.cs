namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class FamilyEntity
{
    public Guid Id { get; private set; }

    public FamilyEntity(Guid id)
        => this.Id = id;
}
