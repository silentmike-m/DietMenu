namespace SilentMike.DietMenu.Core.UnitTests.Hangfire;

using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Core;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;
using SilentMike.DietMenu.Core.UnitTests.Services;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

[TestClass]
public sealed class ImportFamilyDataTests
{
    private readonly Guid familyId = Guid.NewGuid();

    private readonly NullLogger<ImportFamilyData> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenMissingFamilyOnImportFamilyData()
    {
        ImportedFamilyData? importedFamilyDataNotification = null;

        //GIVEN
        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyDataNotification = notification);

        var contextFactory = new DietMenuDbContextFactory();

        var job = new ImportFamilyData(contextFactory.Context, this.logger, this.mediator.Object);

        //WHEN
        Func<Task> action = async () => await job.Run(this.familyId);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(i => i.Code == ErrorCodes.FAMILY_NOT_FOUND)
            ;

        mediator.Verify(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedFamilyDataNotification.Should()
            .NotBeNull()
            ;
        importedFamilyDataNotification!.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.DataArea == "Family"
                && i.Errors.ContainsKey(ErrorCodes.FAMILY_NOT_FOUND)
            );
        importedFamilyDataNotification.IsSuccess.Should()
            .BeFalse()
            ;
        importedFamilyDataNotification.FamilyId.Should()
            .Be(familyId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowCoreNotFoundWhenMissingCoreOnImportFamilyData()
    {
        ImportedFamilyData? importedFamilyDataNotification = null;

        //GIVEN
        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyDataNotification = notification);

        var family = new FamilyEntity(this.familyId);

        var contextFactory = new DietMenuDbContextFactory(family);

        var job = new ImportFamilyData(contextFactory.Context, this.logger, this.mediator.Object);

        //WHEN
        Func<Task> action = async () => await job.Run(this.familyId);

        //THEN
        await action.Should()
                .ThrowAsync<CoreNotFoundException>()
                .Where(i => i.Code == ErrorCodes.CORE_NOT_FOUND)
            ;

        mediator.Verify(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedFamilyDataNotification.Should()
            .NotBeNull()
            ;
        importedFamilyDataNotification!.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.DataArea == "Family"
                && i.Errors.ContainsKey(ErrorCodes.CORE_NOT_FOUND)
            );
        importedFamilyDataNotification.IsSuccess.Should()
            .BeFalse()
            ;
        importedFamilyDataNotification.FamilyId.Should()
            .Be(familyId)
            ;
    }

    [TestMethod]
    public async Task ShouldImportFamilyDataOnImportFamilyData()
    {
        ImportedFamilyData? importedFamilyDataNotification = null;

        //GIVEN
        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyDataNotification = notification);

        var family = new FamilyEntity(this.familyId);

        var core = new CoreEntity(Guid.NewGuid());

        var contextFactory = new DietMenuDbContextFactory(family, core);

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = this.familyId,
        };

        var ingredient = new IngredientEntity(Guid.NewGuid())
        {
            FamilyId = this.familyId,
            TypeId = ingredientType.Id,
        };

        var mealType = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = this.familyId,
        };

        var familyDataToImport = new FamilyDataToImport
        {
            IngredientTypes = new List<IngredientTypeEntity>
            {
                ingredientType,
            },
            Ingredients = new List<IngredientEntity>
            {
                ingredient,
            },
            MealTypes = new List<MealTypeEntity>
            {
                mealType,
            },
        };

        this.mediator.Setup(i => i.Send(It.IsAny<GetFamilyDataToImport>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(familyDataToImport));

        var job = new ImportFamilyData(contextFactory.Context, this.logger, this.mediator.Object);

        //WHEN
        await job.Run(this.familyId);

        //THEN
        mediator.Verify(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedFamilyDataNotification.Should()
            .NotBeNull()
            ;
        importedFamilyDataNotification!.Errors.Should()
            .BeEmpty()
            ;
        importedFamilyDataNotification.IsSuccess.Should()
            .BeTrue()
            ;
        importedFamilyDataNotification.FamilyId.Should()
            .Be(familyId)
            ;

        contextFactory.Context.IngredientTypes.Should()
            .Contain(ingredientType)
            ;
        contextFactory.Context.Ingredients.Should()
            .Contain(ingredient)
            ;
        contextFactory.Context.MealTypes.Should()
            .Contain(mealType)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowExceptionAndSendImportedFamilyDataWithErrors()
    {
        ImportedFamilyData? importedFamilyDataNotification = null;

        //GIVEN
        this.mediator.Setup(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyDataNotification = notification);

        var family = new FamilyEntity(this.familyId);

        var core = new CoreEntity(Guid.NewGuid());

        var contextFactory = new DietMenuDbContextFactory(family, core);

        var exception = new IngredientTypeNotFoundException("internal name");

        var familyDataToImport = new FamilyDataToImport
        {
            Exceptions = new Dictionary<string, ICollection<ApplicationException>>
            {
                { IngredientTypeNames.Fruit, new List<ApplicationException> { exception } },
            },
        };

        this.mediator.Setup(i => i.Send(It.IsAny<GetFamilyDataToImport>(), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(familyDataToImport));

        var job = new ImportFamilyData(contextFactory.Context, this.logger, this.mediator.Object);

        //WHEN
        Func<Task> action = async () => await job.Run(this.familyId);

        //THEN
        await action.Should()
            .ThrowAsync<FamilyDataImportException>()
            .Where(i => i.Exceptions[IngredientTypeNames.Fruit].Contains(exception))
            ;

        mediator.Verify(i => i.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once);

        importedFamilyDataNotification.Should()
            .NotBeNull()
            ;
        importedFamilyDataNotification!.Errors.Should()
            .HaveCount(1)
            .And
            .Contain(i =>
                i.DataArea == IngredientTypeNames.Fruit
                && i.Errors[exception.Code].Contains(exception.Message))
            ;
        importedFamilyDataNotification.IsSuccess.Should()
            .BeFalse()
            ;
        importedFamilyDataNotification.FamilyId.Should()
            .Be(familyId)
            ;

        contextFactory.Context.IngredientTypes.Should()
            .BeEmpty()
            ;
        contextFactory.Context.Ingredients.Should()
            .BeEmpty()
            ;
        contextFactory.Context.MealTypes.Should()
            .BeEmpty()
            ;
    }
}
