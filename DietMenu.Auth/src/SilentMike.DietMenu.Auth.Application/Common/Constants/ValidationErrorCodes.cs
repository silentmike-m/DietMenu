namespace SilentMike.DietMenu.Auth.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string CREATE_FAMILY_EMAIL_INVALID_FORMAT = "CREATE_FAMILY_EMAIL_INVALID_FORMAT";
    public static readonly string CREATE_FAMILY_EMAIL_INVALID_FORMAT_MESSAGE = "Family email must be in correct format";
    public static readonly string CREATE_FAMILY_EMPTY_NAME = "CREATE_FAMILY_EMPTY_NAME";
    public static readonly string CREATE_FAMILY_EMPTY_NAME_MESSAGE = "Family name can not be empty";
    public static readonly string CREATE_USER_EMAIL_INVALID_FORMAT = "CREATE_USER_EMAIL_INVALID_FORMAT";
    public static readonly string CREATE_USER_EMAIL_INVALID_FORMAT_MESSAGE = "User email must be in correct format";
    public static readonly string CREATE_USER_EMPTY_FIRST_NAME = "CREATE_USER_EMPTY_FIRST_NAME";
    public static readonly string CREATE_USER_EMPTY_FIRST_NAME_MESSAGE = "User first name can not be empty";
    public static readonly string CREATE_USER_EMPTY_LAST_NAME = "CREATE_USER_EMPTY_LAST_NAME";
    public static readonly string CREATE_USER_EMPTY_LAST_NAME_MESSAGE = "User last name can not be empty";
    public static readonly string CREATE_USER_EMPTY_PASSWORD = "CREATE_USER_EMPTY_PASSWORD";
    public static readonly string CREATE_USER_EMPTY_PASSWORD_MESSAGE = "User password can not be empty";
}
