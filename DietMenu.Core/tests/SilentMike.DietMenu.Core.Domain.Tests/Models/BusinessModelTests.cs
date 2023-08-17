namespace SilentMike.DietMenu.Core.Domain.Tests.Models;

using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Models;

[TestClass]
public sealed class BusinessModelTests
{
    [TestMethod]
    public void Should_Mark_As_Deleted()
    {
        //GIVEN
        var ingredient = new Ingredient(exchanger: 1.0, Guid.NewGuid(), "ingredient", IngredientTypeNames.Fruit, "kg");

        //WHEN
        ingredient.MarkDeleted();

        //THEN
        ingredient.IsDeleted.Should()
            .BeTrue()
            ;

        ingredient.IsDirty.Should()
            .BeFalse()
            ;

        ingredient.IsNew.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void Should_Mark_As_Dirty()
    {
        //GIVEN
        var ingredient = new Ingredient(exchanger: 1.0, Guid.NewGuid(), "ingredient", IngredientTypeNames.Fruit, "kg");

        //WHEN
        ingredient.MarkDirty();

        //THEN
        ingredient.IsDeleted.Should()
            .BeFalse()
            ;

        ingredient.IsDirty.Should()
            .BeTrue()
            ;

        ingredient.IsNew.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void Should_Mark_As_New()
    {
        //GIVEN
        var ingredient = new Ingredient(exchanger: 1.0, Guid.NewGuid(), "ingredient", IngredientTypeNames.Fruit, "kg");

        //WHEN
        ingredient.MarkNew();

        //THEN
        ingredient.IsDeleted.Should()
            .BeFalse()
            ;

        ingredient.IsDirty.Should()
            .BeFalse()
            ;

        ingredient.IsNew.Should()
            .BeTrue()
            ;
    }

    [TestMethod]
    public void Should_Mark_As_Old()
    {
        //GIVEN
        var ingredient = new Ingredient(exchanger: 1.0, Guid.NewGuid(), "ingredient", IngredientTypeNames.Fruit, "kg");

        //WHEN
        ingredient.MarkDeleted();
        ingredient.MarkOld();

        //THEN
        ingredient.IsDeleted.Should()
            .BeFalse()
            ;

        ingredient.IsDirty.Should()
            .BeFalse()
            ;

        ingredient.IsNew.Should()
            .BeFalse()
            ;
    }
}
