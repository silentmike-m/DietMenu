namespace SilentMike.DietMenu.Core.Domain.Tests.Models;

using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Domain.Models;

[TestClass]
public sealed class IngredientTests
{
    [TestMethod]
    public void Should_Create_Ingredient()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var result = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        //THEN
        result.Exchanger.Should()
            .Be(exchanger)
            ;

        result.FamilyId.Should()
            .Be(familyId)
            ;

        result.Name.Should()
            .Be(name)
            ;

        result.Type.Should()
            .Be(IngredientTypeNames.Fruit)
            ;

        result.UnitSymbol.Should()
            .Be(unitSymbol)
            ;

        result.IsDeleted.Should()
            .BeFalse()
            ;

        result.IsDirty.Should()
            .BeFalse()
            ;

        result.IsNew.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public void Should_Set_Exchanger()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        const double newExchanger = 2.0;

        //WHEN
        ingredient.SetExchanger(newExchanger);

        //THEN
        ingredient.Exchanger.Should()
            .Be(newExchanger)
            ;

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
    public void Should_Set_Name()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        const string newName = "new name";

        //WHEN
        ingredient.SetName(newName);

        //THEN
        ingredient.Name.Should()
            .Be(newName)
            ;

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
    public void Should_Throw_Empty_Name_Exception_On_Create_When_Name_Is_Empty_String()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Create_When_Name_Is_White_Spaces()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "   ";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Set_Name_When_Name_Is_Empty_String()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        const string newName = "";

        //WHEN
        var action = () => ingredient.SetName(newName);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Set_Name_When_Name_Is_White_Spaces()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        const string newName = "   ";

        //WHEN
        var action = () => ingredient.SetName(newName);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Exchanger_On_Create_When_Is_Less_Than_Zero()
    {
        //GIVEN
        const double exchanger = -1.0;
        var familyId = Guid.NewGuid();
        const string name = "ingredient name";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientInvalidExchangerException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_EXCHANGER)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Exchanger_On_Set_Exchanger_When_Is_Less_Than_Zero()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(exchanger, familyId, name, type, unitSymbol);

        const double newExchanger = -1.0;

        //WHEN
        var action = () => ingredient.SetExchanger(newExchanger);

        //THEN
        action.Should()
            .Throw<IngredientInvalidExchangerException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_EXCHANGER)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Type_On_Create_When_Is_Invalid()
    {
        //GIVEN
        const double exchanger = 1.0;
        var familyId = Guid.NewGuid();
        const string name = "ingredient name";
        var type = "invalid";
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientInvalidTypeException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_TYPE)
            ;
    }
}
