namespace SilentMike.DietMenu.Core.Domain.Common.Constants;

public static class MealTypeNames
{
    public static readonly string Dessert = "Dessert";
    public static readonly string Dinner = "Dinner";
    public static readonly string FirstBreakfast = "FirstBreakfast";
    public static readonly string SecondBreakfast = "SecondBreakfast";
    public static readonly string Snack = "Snack";
    public static readonly string Supper = "Supper";

    public static IEnumerable<string> MealTypes
    {
        get
        {
            yield return FirstBreakfast;
            yield return SecondBreakfast;
            yield return Snack;
            yield return Dinner;
            yield return Dessert;
            yield return Supper;
        }
    }
}
