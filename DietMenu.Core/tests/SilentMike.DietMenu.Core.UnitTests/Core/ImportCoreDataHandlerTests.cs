namespace SilentMike.DietMenu.Core.UnitTests.Core;

using SilentMike.DietMenu.Core.Application.Core.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Core.Commands;
using SilentMike.DietMenu.Core.Application.Core.Events;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Application.Exceptions.Core;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[TestClass]
public sealed class ImportCoreDataHandlerTests
{
    private readonly NullLogger<ImportCoreDataHandler> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldImportCoreData()
    {
        ImportedCoreData? importedCoreDataNotification = null;

        //GIVEN
        var contextFactory = new DietMenuDbContextFactory();
        var coreRepository = new CoreRepository(contextFactory.Context);

        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedCoreData>(), CancellationToken.None))
            .Callback<ImportedCoreData, CancellationToken>((notification, _) => importedCoreDataNotification = notification);

        var core = new CoreEntity(Guid.NewGuid());

        var ingredientType = new CoreIngredientTypeEntity(Guid.NewGuid())
        {
            InternalName = "ingredientType_InternalName",
            Name = "ingredientType_Name",
        };

        var ingredient = new CoreIngredientEntity(Guid.NewGuid())
        {
            Exchanger = 1.5m,
            InternalName = "ingredient_InternalName",
            IsActive = true,
            Name = "ingredient_Name",
            TypeId = ingredientType.Id,
            UnitSymbol = "kg",
        };

        var mealType = new CoreMealTypeEntity(Guid.NewGuid())
        {
            InternalName = "mealType_InternalName",
            Name = "mealType_Name",
            Order = 1,
        };

        var response = new CoreDataToImport
        {
            Core = core,
            IngredientTypes = new List<CoreIngredientTypeEntity>
            {
                ingredientType,
            },
            Ingredients = new List<CoreIngredientEntity>
            {
                ingredient,
            },
            MealTypes = new List<CoreMealTypeEntity>
            {
                mealType,
            },
        };

        this.mediator.Setup(i => i.Send(It.IsAny<GetCoreDataToImport>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response));

        this.mediator.Setup(i => i.Send(It.IsAny<GetCoreDataPayloadsToImport>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new CoreDataPayloadsToImport()));

        var request = new ImportCoreData();

        var requestHandler = new ImportCoreDataHandler(coreRepository, this.logger, this.mediator.Object);

        //WHEN
        await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        this.mediator.Verify(i => i.Publish(It.IsAny<ImportedCoreData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedCoreDataNotification.Should()
            .NotBeNull()
            ;
        importedCoreDataNotification!.Errors.Should()
            .BeEmpty()
            ;
        importedCoreDataNotification.IsSuccess.Should()
            .BeTrue()
            ;

        contextFactory.Context.Core.Should()
            .Contain(core)
            ;
        contextFactory.Context.CoreIngredientTypes.Should()
            .Contain(ingredientType)
            ;
        contextFactory.Context.CoreIngredients.Should()
            .Contain(ingredient)
            ;
        contextFactory.Context.CoreMealTypes.Should()
            .Contain(mealType)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowExceptionAndSentImportedCoreDataWithErrors()
    {
        ImportedCoreData? importedCoreDataNotification = null;

        //GIVEN
        var contextFactory = new DietMenuDbContextFactory();
        var coreRepository = new CoreRepository(contextFactory.Context);

        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedCoreData>(), CancellationToken.None))
            .Callback<ImportedCoreData, CancellationToken>((notification, _) => importedCoreDataNotification = notification);

        var core = new CoreEntity(Guid.NewGuid());

        var exception = new IngredientDuplicatedInternalNameException("internal name");

        var response = new CoreDataToImport
        {
            Core = core,
            Exceptions = new Dictionary<string, ICollection<ApplicationException>>
            {
                { IngredientTypeNames.Fruit, new List<ApplicationException> { exception } },
            },
        };

        this.mediator.Setup(i => i.Send(It.IsAny<GetCoreDataToImport>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(response));

        this.mediator.Setup(i => i.Send(It.IsAny<GetCoreDataPayloadsToImport>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new CoreDataPayloadsToImport()));

        var request = new ImportCoreData();

        var requestHandler = new ImportCoreDataHandler(coreRepository, this.logger, this.mediator.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<CoreDataImportException>()
                .Where(i => i.Exceptions[IngredientTypeNames.Fruit].Contains(exception))
            ;

        this.mediator.Verify(i => i.Publish(It.IsAny<ImportedCoreData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedCoreDataNotification.Should()
            .NotBeNull()
            ;
        importedCoreDataNotification!.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.DataArea == IngredientTypeNames.Fruit
                && i.Errors[exception.Code].Contains(exception.Message))
            ;
        importedCoreDataNotification.IsSuccess.Should()
            .BeFalse()
            ;

        contextFactory.Context.Core.Should()
            .BeEmpty()
            ;
        contextFactory.Context.CoreIngredientTypes.Should()
            .BeEmpty()
            ;
        contextFactory.Context.CoreIngredients.Should()
            .BeEmpty()
            ;
        contextFactory.Context.CoreMealTypes.Should()
            .BeEmpty()
            ;
    }
}
