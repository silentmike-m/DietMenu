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
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var result = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        //THEN
        result.Exchanger.Should()
            .Be(exchanger)
            ;

        result.FamilyId.Should()
            .Be(familyId)
            ;

        result.Id.Should()
            .Be(id)
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
    }

    [TestMethod]
    public void Should_Set_Exchanger()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        const decimal newExchanger = 2;

        //WHEN
        ingredient.SetExchanger(newExchanger);

        //THEN
        ingredient.Exchanger.Should()
            .Be(newExchanger)
            ;
    }

    [TestMethod]
    public void Should_Set_Name()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        const string newName = "new name";

        //WHEN
        ingredient.SetName(newName);

        //THEN
        ingredient.Name.Should()
            .Be(newName)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Create_When_Name_Is_Empty_String()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Create_When_Name_Is_White_Spaces()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "   ";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Set_Name_When_Name_Is_Empty_String()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        const string newName = "";

        //WHEN
        var action = () => ingredient.SetName(newName);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Empty_Name_Exception_On_Set_Name_When_Name_Is_White_Spaces()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        const string newName = "   ";

        //WHEN
        var action = () => ingredient.SetName(newName);

        //THEN
        action.Should()
            .Throw<IngredientEmptyNameException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_EMPTY_NAME)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Exchanger_On_Create_When_Is_Less_Than_Zero()
    {
        //GIVEN
        const decimal exchanger = -1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "ingredient name";
        var type = IngredientTypeNames.Fruit.ToLower();
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientInvalidExchangerException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_EXCHANGER)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Exchanger_On_Set_Exchanger_When_Is_Less_Than_Zero()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "new ingredient";
        var type = IngredientTypeNames.Fruit;
        var unitSymbol = string.Empty;

        var ingredient = new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        const decimal newExchanger = -1;

        //WHEN
        var action = () => ingredient.SetExchanger(newExchanger);

        //THEN
        action.Should()
            .Throw<IngredientInvalidExchangerException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_EXCHANGER)
            .Where(exception => exception.Id == id)
            ;
    }

    [TestMethod]
    public void Should_Throw_Invalid_Type_On_Create_When_Is_Invalid()
    {
        //GIVEN
        const decimal exchanger = 1;
        var familyId = Guid.NewGuid();
        var id = Guid.NewGuid();
        const string name = "ingredient name";
        var type = "invalid";
        var unitSymbol = string.Empty;

        //WHEN
        var action = () => new Ingredient(id, exchanger, familyId, name, type, unitSymbol);

        //THEN
        action.Should()
            .Throw<IngredientInvalidTypeException>()
            .Where(exception => exception.Code == ErrorCodes.INGREDIENT_INVALID_TYPE)
            .Where(exception => exception.Id == id)
            ;
    }
}
