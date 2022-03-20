namespace SilentMike.DietMenu.Mailing.UnitTests.Identity;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Application.Identity.Validators;

[TestClass]
public sealed class SendResetPasswordEmailValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var command = new SendResetPasswordEmail
        {
            Email = "test@test.pl",
        };

        var validator = new SendResetPasswordEmailValidator();

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
        var command = new SendResetPasswordEmail
        {
            Email = string.Empty,
        };

        var validator = new SendResetPasswordEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldPassValidationExceptionWhenEmailIsIncorrectFormat()
    {
        //GIVEN
        var command = new SendResetPasswordEmail
        {
            Email = "user.domain.com",
        };

        var validator = new SendResetPasswordEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
