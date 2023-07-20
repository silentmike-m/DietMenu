namespace SilentMike.DietMenu.Auth.Domain.Entities;

using SilentMike.DietMenu.Auth.Domain.Enums;
using SilentMike.DietMenu.Auth.Domain.Exceptions;

public sealed class UserEntity
{
    public string Email { get; private set; }
    public Guid FamilyId { get; private set; }
    public string FirstName { get; internal set; } = null!;
    public Guid Id { get; private set; }
    public string LastName { get; internal set; } = null!;
    public UserRole Role { get; private set; }

    public UserEntity(string email, Guid familyId, Guid id, UserRole role = UserRole.User)
    {
        this.Email = email;
        this.FamilyId = familyId;
        this.Id = id;
        this.Role = role;
    }

    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new UserEmptyFirstNameException(this.Id);
        }

        this.FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new UserEmptyLastNameException(this.Id);
        }

        this.LastName = lastName;
    }
}
