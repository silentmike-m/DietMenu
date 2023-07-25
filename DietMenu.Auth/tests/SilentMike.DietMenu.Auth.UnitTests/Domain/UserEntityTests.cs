namespace SilentMike.DietMenu.Auth.UnitTests.Domain;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Exceptions;

[TestClass]
public sealed class UserEntityTests
{
    [TestMethod]
    public void Should_Set_User_First_Name()
    {
        //GIVEN
        const string newName = "new Name";

        var user = new UserEntity("user@domain.com", Guid.NewGuid(), newName, "last name", Guid.NewGuid());

        //WHEN
        user.SetFirstName(newName);

        //THEN
        user.FirstName.Should()
            .Be(newName)
            ;
    }

    [TestMethod]
    public void Should_Set_User_Last_Name()
    {
        //GIVEN
        const string newName = "new Name";

        var user = new UserEntity("user@domain.com", Guid.NewGuid(), "first name", newName, Guid.NewGuid());

        //WHEN
        user.SetLastName(newName);

        //THEN
        user.LastName.Should()
            .Be(newName)
            ;
    }

    [TestMethod]
    public void Should_Throw_User_Empty_First_Name_Exception_When_First_Name_Is_Empty_String()
    {
        //GIVEN
        const string newName = "";
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new UserEntity("user@domain.com", Guid.NewGuid(), newName, "last name", id);

        //THEN
        action.Should()
            .Throw<UserEmptyFirstNameException>()
            .Where(exception => exception.Code == ErrorCodes.USER_EMPTY_FIRST_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_User_Empty_First_Name_Exception_When_First_Name_Is_White_Spaces()
    {
        //GIVEN
        const string newName = "   ";
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new UserEntity("user@domain.com", Guid.NewGuid(), newName, "last name", id);

        //THEN
        action.Should()
            .Throw<UserEmptyFirstNameException>()
            .Where(exception => exception.Code == ErrorCodes.USER_EMPTY_FIRST_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_User_Empty_Last_Name_Exception_When_Last_Name_Is_Empty_String()
    {
        //GIVEN
        const string newName = "";
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new UserEntity("user@domain.com", Guid.NewGuid(), "first name", newName, id);

        //THEN
        action.Should()
            .Throw<UserEmptyLastNameException>()
            .Where(exception => exception.Code == ErrorCodes.USER_EMPTY_LAST_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_User_Empty_Last_Name_Exception_When_Last_Name_Is_White_Spaces()
    {
        //GIVEN
        const string newName = "   ";
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new UserEntity("user@domain.com", Guid.NewGuid(), "first name", newName, id);

        //THEN
        action.Should()
            .Throw<UserEmptyLastNameException>()
            .Where(exception => exception.Code == ErrorCodes.USER_EMPTY_LAST_NAME)
            .Where(exception => exception.Id == id)
            ;
    }
}
