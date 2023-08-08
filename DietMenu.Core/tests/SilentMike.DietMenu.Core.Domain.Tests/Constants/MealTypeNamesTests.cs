namespace SilentMike.DietMenu.Core.Domain.Tests.Constants;

using System.Reflection;
using SilentMike.DietMenu.Core.Domain.Common.Constants;

[TestClass]
public sealed class MealTypeNamesTests
{
    private static readonly IEnumerable<string> MEAL_TYPE_NAMES = typeof(MealTypeNames)
        .GetFields(BindingFlags.Public | BindingFlags.Static)
        .Where(fieldInfo => fieldInfo.FieldType == typeof(string))
        .Select(fieldInfo => (string)fieldInfo.GetValue(null)!);

    [TestMethod]
    public void Should_Contain_Names_Of_All_Meal_Types()
    {
        //GIVEN

        //WHEN
        var names = MealTypeNames.MealTypes;

        //THEN
        names.Should()
            .BeEquivalentTo(MEAL_TYPE_NAMES)
            ;
    }

    [TestMethod]
    public void Should_Contain_Unique_Meal_Type_Names()
    {
        //GIVEN

        //WHEN

        //THEN
        MEAL_TYPE_NAMES.Should()
            .OnlyHaveUniqueItems()
            ;
    }
}
