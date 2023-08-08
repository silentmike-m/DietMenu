namespace SilentMike.DietMenu.Core.Domain.Common.Constants;

public static class IngredientTypeNames
{
    public static readonly string ComplexCarbohydrate = "ComplexCarbohydrate";
    public static readonly string Fruit = "Fruit";
    public static readonly string HealthyFat = "HealthyFat";
    public static readonly string HighFatProtein = "HighFatProtein";
    public static readonly string LowFatProtein = "LowFatProtein";
    public static readonly string MediumFatProtein = "MediumFatProtein";
    public static readonly string Other = "Other";

    public static IEnumerable<string> IngredientTypes
    {
        get
        {
            yield return ComplexCarbohydrate;
            yield return Fruit;
            yield return HealthyFat;
            yield return HighFatProtein;
            yield return LowFatProtein;
            yield return MediumFatProtein;
            yield return Other;
        }
    }
}
