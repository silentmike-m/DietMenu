namespace SilentMike.DietMenu.Mailing.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string SEND_CREATED_USER_EMPTY_FAMILY_NAME = "send_created_user_empty_family_name";
    public static readonly string SEND_CREATED_USER_EMPTY_FAMILY_NAME_MESSAGE = "Family name can not be empty";
    public static readonly string SEND_CREATED_USER_EMPTY_USER_NAME = "send_created_user_empty_user_name";
    public static readonly string SEND_CREATED_USER_EMPTY_USER_NAME_MESSAGE = "User name can not be empty";
    public static readonly string SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT = "send_created_user_incorrect_email_format";
    public static readonly string SEND_CREATED_USER_INCORRECT_EMAIL_FORMAT_MESSAGE = "Receiver email must be in correct format";
}
