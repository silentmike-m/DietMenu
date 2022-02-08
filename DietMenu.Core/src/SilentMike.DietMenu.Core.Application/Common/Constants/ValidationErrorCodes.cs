namespace SilentMike.DietMenu.Core.Application.Common.Constants;

public static class ValidationErrorCodes
{
    public static readonly string UPSERT_INGREDIENT_EMPTY_NAME = "UPSERT_INGREDIENT_EMPTY_NAME";
    public static readonly string UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE = "Ingredient name can not be empty";
    public static readonly string UPSERT_INGREDIENT_EMPTY_UNIT = "UPSERT_INGREDIENT_EMPTY_UNIT";
    public static readonly string UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE = "Ingredient unit can not be empty";
    public static readonly string UPSERT_INGREDIENT_INVALID_EXCHANGER = "UPSERT_INGREDIENTS_EMPTY_EXCHANGER";
    public static readonly string UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE = "Ingredient exchanger can not be less than 0";

    public static readonly string UPSERT_INGREDIENT_TYPE_EMPTY_NAME = "UPSERT_INGREDIENT_TYPE_EMPTY_NAME";
    public static readonly string UPSERT_INGREDIENT_TYPE_EMPTY_NAME_MESSAGE = "Ingredient type name can not be empty";

    public static readonly string UPSERT_MEAL_TYPE_EMPTY_NAME = "UPSERT_MEAL_TYPE_EMPTY_NAME";
    public static readonly string UPSERT_MEAL_TYPE_EMPTY_NAME_MESSAGE = "Meal type name can not be empty";
    public static readonly string UPSERT_MEAL_TYPE_INVALID_ORDER = "UPSERT_MEAL_TYPE_INVALID_ORDER";
    public static readonly string UPSERT_MEAL_TYPE_INVALID_ORDER_MESSAGE = "Meal type order can not be less than 1";

    public static readonly string UPSERT_RECIPE_EMPTY_NAME = "UPSERT_RECIPE_EMPTY_NAME";
    public static readonly string UPSERT_RECIPE_EMPTY_NAME_MESSAGE = "Recipe name can not be empty";
    public static readonly string UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY = "UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY";
    public static readonly string UPSERT_RECIPE_INGREDIENT_INVALID_QUANTITY_MESSAGE = "Recipe ingredient quantity can not be less than 0";
    public static readonly string UPSERT_RECIPE_INVALID_CARBOHYDRATES = "UPSERT_RECIPE_INVALID_CARBOHYDRATES";
    public static readonly string UPSERT_RECIPE_INVALID_CARBOHYDRATES_MESSAGE = "Recipe carbohydrates can not be less than 0";
    public static readonly string UPSERT_RECIPE_INVALID_ENERGY = "UPSERT_RECIPE_INVALID_ENERGY";
    public static readonly string UPSERT_RECIPE_INVALID_ENERGY_MESSAGE = "Recipe energy can not be less than 0";
    public static readonly string UPSERT_RECIPE_INVALID_FAT = "UPSERT_RECIPE_INVALID_FAT";
    public static readonly string UPSERT_RECIPE_INVALID_FAT_MESSAGE = "Recipe fat can not be less than 0";
    public static readonly string UPSERT_RECIPE_EMPTY_MEAL_TYPE = "UPSERT_RECIPE_EMPTY_MEAL_TYPE";
    public static readonly string UPSERT_RECIPE_EMPTY_MEAL_TYPE_MESSAGE = "Recipe meal type can not be empty";
    public static readonly string UPSERT_RECIPE_INVALID_PROTEIN = "UPSERT_RECIPE_INVALID_PROTEIN";
    public static readonly string UPSERT_RECIPE_INVALID_PROTEIN_MESSAGE = "Recipe protein can not be less than 0";
}
