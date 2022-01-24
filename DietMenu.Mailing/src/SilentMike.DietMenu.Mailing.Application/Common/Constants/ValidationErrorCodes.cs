namespace SilentMike.DietMenu.Mailing.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string SEND_VERIFY_EMAIL_EMPTY_USER_NAME = "SEND_VERIFY_EMAIL_EMPTY_USER_NAME";
    public static readonly string SEND_VERIFY_EMAIL_EMPTY_USER_NAME_MESSAGE = "User name can not be empty";
    public static readonly string SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT = "SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT";
    public static readonly string SEND_VERIFY_EMAIL_INCORRECT_EMAIL_FORMAT_MESSAGE = "Receiver email must be in correct format";
}
