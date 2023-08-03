﻿namespace SilentMike.DietMenu.Auth.UnitTests.Domain;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Domain.Common.Constants;
using SilentMike.DietMenu.Auth.Domain.Entities;
using SilentMike.DietMenu.Auth.Domain.Exceptions;

[TestClass]
public sealed class FamilyEntityTests
{
    [TestMethod]
    public void Should_Set_Family_Name()
    {
        //GIVEN
        const string newName = "new Name";

        var family = new FamilyEntity(Guid.NewGuid(), "name");

        //WHEN
        family.SetName(newName);

        //THEN
        family.Name.Should()
            .Be(newName)
            ;
    }

    [TestMethod]
    public void Should_Throw_Family_Empty_Name_Exception_When_Name_Is_Empty_String()
    {
        //GIVEN
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new FamilyEntity(id, "");

        //THEN
        action.Should()
            .Throw<FamilyEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.FAMILY_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Family_Empty_Name_Exception_When_Name_Is_White_Spaces()
    {
        //GIVEN
        var id = Guid.NewGuid();

        //WHEN
        var action = () => new FamilyEntity(id, "    ");

        //THEN
        action.Should()
            .Throw<FamilyEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.FAMILY_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }
}
