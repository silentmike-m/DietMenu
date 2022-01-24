namespace SilentMike.DietMenu.Core.UnitTests.Auth;

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Validators;
using SilentMike.DietMenu.Core.Application.Auth.ViewModels;
using SilentMike.DietMenu.Core.Application.Common.Constants;

[TestClass]
public sealed class CreateUserValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "test@test.pl",
            FamilyName = "FamilyName",
            FirstName = "FirstName",
            Id = Guid.NewGuid(),
            LastName = "LastName",
            Password = "Password",
            UserName = "UserName",
        };

        var command = new CreateUser
        {
            CreateCode = "CreateCode",
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var validationResult = validator.Validate(command);

        //THEN
        validationResult.Errors.Should()
            .BeEmpty()
            ;
        validationResult.IsValid.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void ShouldThrowValidationException()
    {
        //GIVEN
        var command = new CreateUser();

        var validator = new CreateUserValidator();

        //WHEN
        var validationResult = validator.Validate(command);

        //THEN
        validationResult.Errors.Should()
            .NotBeEmpty()
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_CREATE_CODE
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_CREATE_CODE_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_FAMILY_NAME
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_FAMILY_NAME_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_FIRST_NAME_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_ID
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_ID_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_PASSWORD_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_EMPTY_USER_NAME
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_EMPTY_USER_NAME_MESSAGE)
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        validationResult.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldThrowEmailValidationExceptionWhenInvalidEmailFormat()
    {
        //GIVEN
        var userToCreate = new UserToCreate
        {
            Email = "test.test.pl",
            FamilyName = "FamilyName",
            FirstName = "FirstName",
            Id = Guid.NewGuid(),
            LastName = "LastName",
            Password = "Password",
            UserName = "UserName",
        };

        var command = new CreateUser
        {
            CreateCode = "CreateCode",
            User = userToCreate,
        };

        var validator = new CreateUserValidator();

        //WHEN
        var validationResult = validator.Validate(command);

        //THEN
        validationResult.Errors.Should()
            .NotBeEmpty()
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT
                && i.ErrorMessage == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        validationResult.IsValid.Should()
            .BeFalse()
            ;
    }
}
