namespace SilentMike.DietMenu.Core.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string CREATE_INGREDIENT_EMPTY_NAME = "CREATE_INGREDIENT_EMPTY_NAME";
    public static readonly string CREATE_INGREDIENT_EMPTY_NAME_MESSAGE = "Ingredient name can not be empty";
    public static readonly string CREATE_INGREDIENT_INVALID_EXCHANGER = "CREATE_INGREDIENT_INVALID_EXCHANGER";
    public static readonly string CREATE_INGREDIENT_INVALID_EXCHANGER_MESSAGE = "Ingredient exchanger can not be less than 0";
    public static readonly string CREATE_INGREDIENT_INVALID_TYPE = "CREATE_INGREDIENT_INVALID_TYPE";
    public static readonly string CREATE_INGREDIENT_INVALID_TYPE_MESSAGE = "Ingredient type is invalid";
}
