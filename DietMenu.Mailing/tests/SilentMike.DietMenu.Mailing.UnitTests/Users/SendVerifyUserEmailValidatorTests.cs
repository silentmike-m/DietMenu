namespace SilentMike.DietMenu.Mailing.UnitTests.Users;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;
using SilentMike.DietMenu.Mailing.Application.Users.Validators;

[TestClass]
public sealed class SendVerifyUserEmailValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
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
    public void ShouldThrowValidationExceptionWhenParametersAreEmpty()
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
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void ShouldPassValidationExceptionWhenEmailIsIncorrectFormat()
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
            .Contain(i => i.ErrorCode == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT
                          && i.ErrorMessage == ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        result.IsValid.Should()
            .BeFalse()
            ;
    }
}
