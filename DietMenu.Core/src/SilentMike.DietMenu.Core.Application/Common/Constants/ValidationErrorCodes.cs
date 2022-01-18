namespace SilentMike.DietMenu.Core.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string CREATE_USER_EMPTY_CREATE_CODE = "CREATE_USER_EMPTY_CREATE_CODE";
    public static readonly string CREATE_USER_EMPTY_CREATE_CODE_MESSAGE = "Missing creation code";
    public static readonly string CREATE_USER_EMPTY_FAMILY_NAME = "CREATE_USER_EMPTY_FAMILY_NAME";
    public static readonly string CREATE_USER_EMPTY_FAMILY_NAME_MESSAGE = "User family name can not be empty";
    public static readonly string CREATE_USER_EMPTY_FIRST_NAME = "CREATE_USER_EMPTY_FIRST_NAME";
    public static readonly string CREATE_USER_EMPTY_FIRST_NAME_MESSAGE = "First name can not be empty";
    public static readonly string CREATE_USER_EMPTY_ID = "CREATE_USER_EMPTY_ID";
    public static readonly string CREATE_USER_EMPTY_ID_MESSAGE = "Missing identifier";
    public static readonly string CREATE_USER_EMPTY_PASSWORD = "CREATE_USER_EMPTY_PASSWORD";
    public static readonly string CREATE_USER_EMPTY_PASSWORD_MESSAGE = "Password can not be empty";
    public static readonly string CREATE_USER_EMPTY_USER_NAME = "CREATE_USER_EMPTY_USER_NAME";
    public static readonly string CREATE_USER_EMPTY_USER_NAME_MESSAGE = "User name can not be empty";
    public static readonly string CREATE_USER_INCORRECT_CREATE_CODE = "CREATE_USER_INCORRECT_CREATE_CODE";
    public static readonly string CREATE_USER_INCORRECT_CREATE_CODE_MESSAGE = "Invalid create user code";
    public static readonly string CREATE_USER_INCORRECT_EMAIL_FORMAT = "CREATE_USER_INCORRECT_EMAIL_FORMAT";
    public static readonly string CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE = "User email must be in correct format";
}
