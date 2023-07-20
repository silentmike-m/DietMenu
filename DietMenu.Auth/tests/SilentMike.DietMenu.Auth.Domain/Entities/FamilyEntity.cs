namespace SilentMike.DietMenu.Auth.Domain.Entities;

using SilentMike.DietMenu.Auth.Domain.Exceptions;

public sealed class FamilyEntity
{
    public Guid Id { get; private set; }
    public string Name { get; internal set; } = string.Empty;

    public FamilyEntity(Guid id)
        => this.Id = id;

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new FamilyEmptyNameException(this.Id);
        }

        this.Name = name;
    }
}
