namespace SilentMike.DietMenu.Core.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string UPSERT_INGREDIENTS_EMPTY_NAME = "UPSERT_INGREDIENTS_EMPTY_NAME";
    public static readonly string UPSERT_INGREDIENTS_EMPTY_NAME_MESSAGE = "Ingredient name can not be empty";
    public static readonly string UPSERT_INGREDIENTS_EMPTY_UNIT = "UPSERT_INGREDIENTS_EMPTY_UNIT";
    public static readonly string UPSERT_INGREDIENTS_EMPTY_UNIT_MESSAGE = "Ingredient unit can not be empty";
    public static readonly string UPSERT_INGREDIENTS_INVALID_EXCHANGER = "UPSERT_INGREDIENTS_EMPTY_EXCHANGER";
    public static readonly string UPSERT_INGREDIENTS_INVALID_EXCHANGER_MESSAGE = "Ingredient exchanger can not less than 0";

    public static readonly string UPSERT_INGREDIENT_TYPES_EMPTY_NAME = "UPSERT_INGREDIENT_TYPES_EMPTY_NAME";
    public static readonly string UPSERT_INGREDIENT_TYPES_EMPTY_NAME_MESSAGE = "Ingredient type name can not be empty";

    public static readonly string UPSERT_MEAL_TYPES_EMPTY_NAME = "UPSERT_MEAL_TYPES_EMPTY_NAME";
    public static readonly string UPSERT_MEAL_TYPES_EMPTY_NAME_MESSAGE = "Meal type name can not be empty";
    public static readonly string UPSERT_MEAL_TYPES_INVALID_ORDER = "UPSERT_MEAL_TYPES_INVALID_ORDER";
    public static readonly string UPSERT_MEAL_TYPES_INVALID_ORDER_MESSAGE = "Meal type order can not be less than 1";
}
