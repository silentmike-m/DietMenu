namespace SilentMike.DietMenu.Core.UnitTests.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.PostProcessors;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;

[TestClass]
public sealed class GetCoreIngredientsToImportPostProcessorTests
{
    private const string INGREDIENT_TYPE_INTERNAL_NAME = "Fruit";

    private readonly string defaultDataName = DataNames.Ingredients;

    private readonly Guid ingredientTypeId = Guid.NewGuid();

    private readonly NullLogger<GetCoreIngredientsToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldReturnCoreDataImportEmptyPayloadExceptionWhenIngredientsPayloadIsEmpty()
    {
        //GIVEN
        var request = new GetCoreDataToImport();

        var response = new CoreDataToImport();

        var processor = new GetCoreIngredientsToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(
            this.logger,
            this.mediator.Object);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Exceptions[this.defaultDataName].Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Code == ErrorCodes.CORE_DATA_IMPORT_EMPTY_PAYLOAD)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnIngredientEmptyInternalNameExceptionWhenExcelIngredientInternalNameIsEmpty()
    {
        //GIVEN
        var ingredientToImport = new IngredientToImport
        {
            InternalName = string.Empty,
        };

        IReadOnlyList<IngredientToImport> ingredientsToImport = new List<IngredientToImport>
        {
            ingredientToImport,
        };

        this.mediator.Setup(i => i.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(ingredientsToImport));

        var request = new GetCoreDataToImport
        {
            IngredientsPayload = new byte[10],
        };

        var response = new CoreDataToImport
        {
            IngredientTypes = new List<CoreIngredientTypeEntity>
            {
                new(this.ingredientTypeId)
                {
                    InternalName = INGREDIENT_TYPE_INTERNAL_NAME,
                },
            },
        };

        var processor = new GetCoreIngredientsToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(
            this.logger,
            this.mediator.Object);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Exceptions[INGREDIENT_TYPE_INTERNAL_NAME].Should()
            .HaveCount(1)
            .And
            .Contain(i => i.Code == ErrorCodes.INGREDIENT_EMPTY_INTERNAL_NAME)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnIngredientDuplicatedInternalNameExceptionWhenExcelIngredientHaveDuplicatedInternalName()
    {
        //GIVEN
        var ingredientOneToImport = new IngredientToImport
        {
            InternalName = "InternalName",
        };

        var ingredientTwoToImport = new IngredientToImport
        {
            InternalName = "InternalName",
        };

        IReadOnlyList<IngredientToImport> ingredientsToImport = new List<IngredientToImport>
        {
            ingredientOneToImport,
            ingredientTwoToImport,
        };

        this.mediator.Setup(i => i.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(ingredientsToImport));

        var request = new GetCoreDataToImport
        {
            IngredientsPayload = new byte[10],
        };

        var response = new CoreDataToImport
        {
            IngredientTypes = new List<CoreIngredientTypeEntity>
            {
                new(this.ingredientTypeId)
                {
                    InternalName = INGREDIENT_TYPE_INTERNAL_NAME,
                },
            },
        };

        var processor = new GetCoreIngredientsToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(
            this.logger,
            this.mediator.Object);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Exceptions[INGREDIENT_TYPE_INTERNAL_NAME].Should()
            .Contain(i => i.Code == ErrorCodes.INGREDIENT_DUPLICATED_INTERNAL_NAME)
            ;
    }

    [TestMethod]
    public async Task ShouldImportCoreIngredients()
    {
        //GIVEN
        var ingredientOneToImport = new IngredientToImport
        {
            Exchanger = 12.5m,
            InternalName = "ingredientOneToImport",
            Name = "ingredientOneToImport",
            UnitSymbol = "kg",
        };

        var ingredientTwoToImport = new IngredientToImport
        {
            Exchanger = 33.3m,
            InternalName = "ingredientTwoToImport",
            Name = "ingredientTwoToImport",
            UnitSymbol = "szt",
        };

        IReadOnlyList<IngredientToImport> ingredientsToImport = new List<IngredientToImport>
        {
            ingredientOneToImport,
            ingredientTwoToImport,
        };

        this.mediator.Setup(i => i.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(ingredientsToImport));

        var request = new GetCoreDataToImport
        {
            IngredientsPayload = new byte[10],
        };

        var response = new CoreDataToImport
        {
            IngredientTypes = new List<CoreIngredientTypeEntity>
            {
                new(this.ingredientTypeId)
                {
                    InternalName = INGREDIENT_TYPE_INTERNAL_NAME,
                },
            },
        };

        var processor = new GetCoreIngredientsToImportPostProcessor<GetCoreDataToImport, ICoreDataToImport>(
            this.logger,
            this.mediator.Object);

        //WHEN
        await processor.Process(request, response, CancellationToken.None);

        //THEN
        response.Core[INGREDIENT_TYPE_INTERNAL_NAME].Should()
            .NotBeEmpty()
            ;

        response.Ingredients.Should()
            .Contain(i =>
                i.Exchanger == ingredientOneToImport.Exchanger
                && i.InternalName == ingredientOneToImport.InternalName
                && i.IsActive == true
                && i.Name == ingredientOneToImport.Name
                && i.TypeId == this.ingredientTypeId
                && i.UnitSymbol == ingredientOneToImport.UnitSymbol
                )
            .And
            .Contain(i =>
                i.Exchanger == ingredientTwoToImport.Exchanger
                && i.InternalName == ingredientTwoToImport.InternalName
                && i.IsActive == true
                && i.Name == ingredientTwoToImport.Name
                && i.TypeId == this.ingredientTypeId
                && i.UnitSymbol == ingredientTwoToImport.UnitSymbol
            )
            ;
    }
}
