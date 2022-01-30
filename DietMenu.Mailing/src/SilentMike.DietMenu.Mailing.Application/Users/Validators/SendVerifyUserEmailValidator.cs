﻿namespace SilentMike.DietMenu.Mailing.Application.Users.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Mailing.Application.Users.Commands;

public sealed class SendVerifyUserEmailValidator : AbstractValidator<SendVerifyUserEmail>
{
    public SendVerifyUserEmailValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_VERIFY_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;
    }
}
