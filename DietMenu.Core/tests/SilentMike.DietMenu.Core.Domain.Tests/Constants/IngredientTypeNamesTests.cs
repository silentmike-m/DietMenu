namespace SilentMike.DietMenu.Core.Domain.Tests.Constants;

using System.Reflection;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[TestClass]
public sealed class IngredientTypeNamesTests
{
    private static readonly IEnumerable<string> INGREDIENT_TYPE_NAMES = typeof(IngredientTypeNames)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(fieldInfo => fieldInfo.FieldType == typeof(string))
        .Select(fieldInfo => (string)fieldInfo.GetValue(null)!);

    [TestMethod]
    public void Should_Contain_Names_Of_All_Ingredient_Types()
    {
        //GIVEN

        //WHEN
        var names = IngredientTypeNames.IngredientTypes;

        //THEN
        names.Should()
            .BeEquivalentTo(INGREDIENT_TYPE_NAMES)
            ;
    }

    [TestMethod]
    public void Should_Contain_Unique_Ingredient_Type_Names()
    {
        //GIVEN

        //WHEN

        //THEN
        INGREDIENT_TYPE_NAMES.Should()
            .OnlyHaveUniqueItems()
            ;
    }
}
