namespace SilentMike.DietMenu.Mailing.UnitTests.Identity;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Identity.Commands;
using SilentMike.DietMenu.Mailing.Application.Identity.Validators;

[TestClass]
public sealed class SendVerifyUserEmailValidatorTests
{
    [TestMethod]
    public void Should_Pass_Validation()
    {
        //GIVEN
        var command = new SendVerifyUserEmail
        {
            Email = "test@test.pl",
        };

        var validator = new SendVerifyUserEmailValidator();

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
    public void Should_Throw_Validation_Exception_When_Parameters_Are_Empty()
    {
        //GIVEN
        var command = new SendVerifyUserEmail
        {
            Email = string.Empty,
        };

        var validator = new SendVerifyUserEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(2)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT
                && failure.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldP_Throw_Validation_Exception_When_Email_Is_Incorrect_Format()
    {
        //GIVEN
        var command = new SendVerifyUserEmail
        {
            Email = "user.domain.com",
        };

        var validator = new SendVerifyUserEmailValidator();

        //WHEN
        var result = validator.Validate(command);

        //THEN
        result.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(failure =>
                failure.ErrorCode == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT
                && failure.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
