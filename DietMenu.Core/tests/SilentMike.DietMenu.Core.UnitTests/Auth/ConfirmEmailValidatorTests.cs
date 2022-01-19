namespace SilentMike.DietMenu.Core.UnitTests.Auth;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Validators;
using SilentMike.DietMenu.Core.Application.Common.Constants;

[TestClass]
public sealed class ConfirmEmailValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var command = new ConfirmEmail
        {
            Email = "user@domain.com",
            Token = "token",
        };

        var validator = new ConfirmEmailValidator();

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
        var command = new ConfirmEmail
        {
            Email = "user.domain.com",
            Token = string.Empty,
        };

        var validator = new ConfirmEmailValidator();

        //WHEN
        var validationResult = validator.Validate(command);

        //THEN
        validationResult.Errors.Should()
                .HaveCount(2)
                .And
                .Contain(i =>
                    i.ErrorCode == ValidationErrorCodes.CONFIRM_EMAIL_EMAIL_FORMAT
                    && i.ErrorMessage == ValidationErrorCodes.CONFIRM_EMAIL_EMAIL_FORMAT_MESSAGE)
                .And
                .Contain(i =>
                    i.ErrorCode == ValidationErrorCodes.CONFIRM_EMAIL_EMPTY_TOKEN
                    && i.ErrorMessage == ValidationErrorCodes.CONFIRM_EMAIL_EMPTY_TOKEN_MESSAGE)
                ;

        validationResult.IsValid.Should()
            .BeFalse()
            ;
    }
}
