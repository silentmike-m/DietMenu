namespace SilentMike.DietMenu.Mailing.Infrastructure.MassTransit.Validators;

using FluentValidation;
using SilentMike.DietMenu.Mailing.Application.Common.Constants;
using SilentMike.DietMenu.Shared.MassTransit;

public sealed class SendCreatedUserMessageValidator : AbstractValidator<ISendCreatedUserMessage>
{
    public SendCreatedUserMessageValidator()
    {
        RuleFor(i => i.Email)
            .NotEmpty()
            .EmailAddress()
            .WithErrorCode(ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT)
            .WithMessage(ValidationErrorCodes.SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT_MESSAGE)
            ;

        RuleFor(i => i.FamilyName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.SEND_CREATED_USER_EMPTY_FAMILY_NAME)
            .WithMessage(ValidationErrorCodes.SEND_CREATED_USER_EMPTY_FAMILY_NAME_MESSAGE)
            ;

        RuleFor(i => i.UserName)
            .NotEmpty()
            .WithErrorCode(ValidationErrorCodes.SEND_CREATED_USER_EMPTY_USER_NAME)
            .WithMessage(ValidationErrorCodes.SEND_CREATED_USER_EMPTY_USER_NAME_MESSAGE)
            ;
    }
}
