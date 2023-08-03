namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using SilentMike.DietMenu.Auth.Domain.Entities;

internal static class UserEntityFactory
{
    private const string EMAIL = "user@domain.com";
    private const string FIRST_NAME = "John";
    private const string LAST_NAME = "Wick";

    public static UserEntity Create(Guid familyId, Guid id)
    {
        var result = new UserEntity(EMAIL, familyId, FIRST_NAME, LAST_NAME, id);

        return result;
    }
}
