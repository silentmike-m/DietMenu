namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Application.Users.Validators;

[TestClass]
public sealed class CreateUserValidatorTest
{
    private const string INCORRECT_USER_EMAIL = "user#domain.com";
    private const string USER_EMAIL = "user@domain.com";

    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var query = new CreateUser
        {
            Email = USER_EMAIL,
            Family = "family",
            FirstName = "John",
            Password = "password",
            RegisterCode = "register code",
        };

        //WHEN
        var createUserValidator = new CreateUserValidator();
        var validationResult = createUserValidator.Validate(query);

        //THEN
        validationResult.Errors.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public void ShouldThrowValidationExceptionWhenAllRequiredPropertiesAreEmptyOrSpaces()
    {
        //GIVEN
        var query = new CreateUser
        {
            Email = string.Empty,
            Family = " ",
            FirstName = string.Empty,
            Password = string.Empty,
            RegisterCode = "          ",
        };

        //WHEN
        var createUserValidator = new CreateUserValidator();
        var validationResult = createUserValidator.Validate(query);

        //THEN
        validationResult.Errors.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(6)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_MISSING_FAMILY)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_MISSING_FIRST_NAME)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_MISSING_PASSWORD)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_MISSING_REGISTER_CODE)
            ;
    }

    [TestMethod]
    public void ShouldThrowValidationExceptionWhenEmailIsIncorrect()
    {
        //GIVEN
        var query = new CreateUser
        {
            Email = INCORRECT_USER_EMAIL,
            Family = "family",
            FirstName = "John",
            Password = "password",
            RegisterCode = "register code",
        };

        //WHEN
        var createUserValidator = new CreateUserValidator();
        var validationResult = createUserValidator.Validate(query);

        //THEN
        validationResult.Errors.Should()
            .NotBeNullOrEmpty()
            .And
            .HaveCount(1)
            .And
            .Contain(e => e.ErrorCode == ValidationErrorCodes.CREATE_USER_INCORRECT_EMAIL_FORMAT)
            ;
    }
}
