namespace SilentMike.DietMenu.Auth.UnitTests.Domain;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Exceptions;

[TestClass]
public sealed class FamilyEntityTests
{
    [DataTestMethod, DataRow(""), DataRow(" ")]
    public void Should_Throw_Family_Empty_Email_Exception_When_Email_Is_Empty(string email)
    {
        //GIVEN
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new FamilyEntity(id, email, "family");

        //THEN
        action.Should()
            .Throw<FamilyEmptyEmailException>()
            .Where(exception => exception.Code == ErrorCodes.FAMILY_EMPTY_EMAIL)
            .Where(exception => exception.Id == id)
            ;
    }

    [DataTestMethod, DataRow(""), DataRow(" ")]
    public void Should_Throw_Family_Empty_Name_Exception_When_Name_Is_Empty(string name)
    {
        //GIVEN
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new FamilyEntity(id, "family@domain.com", name);

        //THEN
        action.Should()
            .Throw<FamilyEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.FAMILY_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }
}
