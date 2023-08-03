namespace SilentMike.DietMenu.Auth.Domain.Entities;

using SilentMike.DietMenu.Auth.Domain.Exceptions;

public sealed class FamilyEntity
{
    public Guid Id { get; private set; }
    public bool IsConfirmed { get; set; } = default;
    public string Name { get; private set; } = string.Empty;

    public FamilyEntity(Guid id, string name)
    {
        this.Id = id;
        this.SetName(name);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new FamilyEmptyNameException(this.Id);
        }

        this.Name = name;
    }
}
