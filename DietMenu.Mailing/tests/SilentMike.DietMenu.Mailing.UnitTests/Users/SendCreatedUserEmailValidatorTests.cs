namespace SilentMike.DietMenu.Mailing.UnitTests.Users;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Mailing.Application.Users.Validators;

[TestClass]
public sealed class SendCreatedUserEmailValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var command = new SendCreatedUserEmail
        {
            Email = "test@test.pl",
            FamilyName = "FamilyName",
            UserName = "UserName",
        };

        var validator = new SendCreatedUserEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .BeEmpty();
        result.IsValid.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void ShouldThrowValidationExceptionWhenParametersAreEmpty()
    {
        //GIVEN
        var command = new SendCreatedUserEmail
        {
            Email = string.Empty,
            FamilyName = string.Empty,
            UserName = string.Empty,
        };

        var validator = new SendCreatedUserEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(4)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_CREATED_USER_EMPTY_FAMILY_NAME
                          && i.ErrorMessage == ValidationErrorCodes.SEND_CREATED_USER_EMPTY_FAMILY_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_CREATED_USER_EMPTY_USER_NAME
                          && i.ErrorMessage == ValidationErrorCodes.SEND_CREATED_USER_EMPTY_USER_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldPassValidationExceptionWhenEmailIsIncorrectFormat()
    {
        //GIVEN
        var command = new SendCreatedUserEmail
        {
            Email = "test.test.pl",
            FamilyName = "FamilyName",
            UserName = "UserName",
        };

        var validator = new SendCreatedUserEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
