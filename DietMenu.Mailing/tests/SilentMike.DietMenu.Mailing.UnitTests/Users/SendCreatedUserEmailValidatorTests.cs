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
        var command = new SendVerifyEmail
        {
            Email = "test@test.pl",
            UserName = "UserName",
        };

        var validator = new SendVerifyEmailValidator();

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
        var command = new SendVerifyEmail
        {
            Email = string.Empty,
            UserName = string.Empty,
        };

        var validator = new SendVerifyEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(3)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_VERIFY_EMAIL_EMPTY_USER_NAME
                          && i.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_EMAIL_EMPTY_USER_NAME_MESSAGE)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldPassValidationExceptionWhenEmailIsIncorrectFormat()
    {
        //GIVEN
        var command = new SendVerifyEmail
        {
            Email = "test.test.pl",
            UserName = "UserName",
        };

        var validator = new SendVerifyEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
