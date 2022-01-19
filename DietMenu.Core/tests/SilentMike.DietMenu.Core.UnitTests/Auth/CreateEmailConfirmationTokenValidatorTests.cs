namespace SilentMike.DietMenu.Core.UnitTests.Auth;

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Validators;
using SilentMike.DietMenu.Core.Application.Common.Constants;

[TestClass]
public sealed class CreateEmailConfirmationTokenValidatorTests
{
    [TestMethod]
    public void ShouldPassValidation()
    {
        //GIVEN
        var command = new CreateEmailConfirmationToken
        {
            Email = "user@domain.com",
        };

        var validator = new CreateEmailConfirmationTokenValidator();

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
    public void ShouldThrowEmailValidationExceptionWhenInvalidEmailFormat()
    {
        //GIVEN
        var command = new CreateEmailConfirmationToken
        {
            Email = "user.domain.com",
        };

        var validator = new CreateEmailConfirmationTokenValidator();

        //WHEN
        var validationResult = validator.Validate(command);

        //THEN
        validationResult.Errors.Should()
            .NotBeEmpty()
            .And
            .Contain(i =>
                i.ErrorCode == ValidationErrorCodes.CREATE_EMAIL_CONFIRMATION_TOKEN_INCORRECT_EMAIL_FORMAT
                && i.ErrorMessage == ValidationErrorCodes.CREATE_EMAIL_CONFIRMATION_TOKEN_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        validationResult.IsValid.Should()
            .BeFalse()
            ;
    }
}
