namespace SilentMike.DietMenu.Auth.Domain.Entities;

using SilentMike.DietMenu.Auth.Domain.Exceptions;

public sealed class FamilyEntity
{
    public string Email { get; set; } = string.Empty;
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public FamilyEntity(Guid id, string email, string name)
    {
        this.Id = id;
        this.SetEmail(email);
        this.SetName(name);
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new FamilyEmptyEmailException(this.Id);
        }

        this.Email = email;
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
