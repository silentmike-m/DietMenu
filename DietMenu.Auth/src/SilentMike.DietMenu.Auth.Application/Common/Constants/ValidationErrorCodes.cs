namespace SilentMike.DietMenu.Auth.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string CREATE_USER_INCORRECT_EMAIL_FORMAT = "CREATE_USER_INCORRECT_EMAIL_FORMAT";
    public static readonly string CREATE_USER_INCORRECT_EMAIL_FORMAT_MESSAGE = "User email must be in correct format";
    public static readonly string CREATE_USER_MISSING_FAMILY = "CREATE_USER_MISSING_FAMILY";
    public static readonly string CREATE_USER_MISSING_FAMILY_MESSAGE = "User family cannot be empty";
    public static readonly string CREATE_USER_MISSING_FIRST_NAME = "CREATE_USER_MISSING_FIRST_NAME";
    public static readonly string CREATE_USER_MISSING_FIRST_NAME_MESSAGE = "User first name cannot be empty";
    public static readonly string CREATE_USER_MISSING_PASSWORD = "CREATE_USER_MISSING_PASSWORD";
    public static readonly string CREATE_USER_MISSING_PASSWORD_MESSAGE = "User password cannot be empty";
    public static readonly string CREATE_USER_MISSING_REGISTER_CODE = "CREATE_USER_MISSING_REGISTER_CODE";
    public static readonly string CREATE_USER_MISSING_REGISTER_CODE_MESSAGE = "Register code cannot be empty";
}
