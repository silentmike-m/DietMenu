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
    public void Should_Pass_Validation()
    {
        //GIVEN
        var request = new SendResetPasswordEmail
        {
            Email = "test@test.pl",
            Url = "url",
        };

        var validator = new SendResetPasswordEmailValidator();

        //WHEN
        var result = validator.Validate(request);

        //THEN
        result.Errors.Should()
            .BeEmpty();

        result.IsValid.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_throw_Validation_Exception_When_Email_Is_Incorrect_Format()
    {
        //GIVEN
        var request = new SendResetPasswordEmail
        {
            Email = "user.domain.com",
            Url = "url",
        };

        var validator = new SendResetPasswordEmailValidator();

        //WHEN
        var result = validator.Validate(request);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT
                && failure.ErrorMessage == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void Should_Throw_Validation_Exception_When_Parameters_Are_Empty()
    {
        //GIVEN
        var request = new SendResetPasswordEmail
        {
            Email = string.Empty,
            Url = "url",
        };

        var validator = new SendResetPasswordEmailValidator();

        //WHEN
        var result = validator.Validate(request);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT
                && failure.ErrorMessage == ValidationErrorCodes.SEND_RESET_PASSWORD_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
