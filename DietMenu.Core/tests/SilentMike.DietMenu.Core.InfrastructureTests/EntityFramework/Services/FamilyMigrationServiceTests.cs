namespace SilentMike.DietMenu.Core.InfrastructureTests.EntityFramework.Services;

using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.EPPlus;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Families;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.InfrastructureTests.Helpers;

[TestClass]
public sealed class FamilyMigrationServiceTests
{
    private readonly NullLogger<FamilyMigrationService> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task Should_Create_Family_And_Import_Ingredients()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var familyId = Guid.NewGuid();
        var payload = new byte[128];

        var fruitToImport = new IngredientToImport(Exchanger: 1.2, Guid.NewGuid(), "fruit", "kg");

        var healthFatToImport = new IngredientToImport(Exchanger: 2.7, Guid.NewGuid(), "health fat", "g");

        var emptyIngredientsHashString = new List<IngredientToImport>().GetHashString();

        var fruitsToImportHashString = new List<IngredientToImport>
        {
            fruitToImport,
        }.GetHashString();

        var healthFatsToImportHashString = new List<IngredientToImport>
        {
            healthFatToImport,
        }.GetHashString();

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        this.mediator
            .Setup(service => service.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParseIngredientsFromExcelFile request, CancellationToken _) =>
            {
                if (request.Payload != payload)
                {
                    throw new ArgumentException();
                }

                if (request.IngredientType == IngredientTypeNames.Fruit)
                {
                    return new List<IngredientToImport>
                    {
                        fruitToImport,
                    };
                }

                if (request.IngredientType == IngredientTypeNames.HealthyFat)
                {
                    return new List<IngredientToImport>
                    {
                        healthFatToImport,
                    };
                }

                return new List<IngredientToImport>();
            });

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext();

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedResults = IngredientTypeNames.IngredientTypes
            .Select(ingredientType => new ImportFamilyDataResult
            {
                DataArea = ingredientType,
            }).ToList();

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = familyId,
            Results = expectedResults,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        var expectedFamily = new FamilyEntity
        {
            FamilyId = familyId,
            IngredientsVersion = IngredientTypeNames.IngredientTypes
                .ToDictionary(key => key, value =>
                {
                    if (value == IngredientTypeNames.Fruit)
                    {
                        return fruitsToImportHashString;
                    }

                    if (value == IngredientTypeNames.HealthyFat)
                    {
                        return healthFatsToImportHashString;
                    }

                    return emptyIngredientsHashString;
                }),
        };

        var createdFamily = context.Context!.Families.SingleOrDefault(family => family.FamilyId == familyId);

        createdFamily.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedFamily, options => options.Excluding(family => family.Id))
            ;

        var expectedFruit = new IngredientEntity
        {
            Exchanger = fruitToImport.Exchanger,
            FamilyId = familyId,
            IngredientId = fruitToImport.Id,
            IsActive = true,
            IsSystem = true,
            Name = fruitToImport.Name,
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = fruitToImport.UnitSymbol,
        };

        var createdFruit = context.Context!.Ingredients.SingleOrDefault(ingredient => ingredient.IngredientId == fruitToImport.Id);

        createdFruit.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedFruit, options => options.Excluding(ingredient => ingredient.Id))
            ;

        var expectedHealthFat = new IngredientEntity
        {
            Exchanger = healthFatToImport.Exchanger,
            FamilyId = familyId,
            IngredientId = healthFatToImport.Id,
            IsActive = true,
            IsSystem = true,
            Name = healthFatToImport.Name,
            Type = IngredientTypeNames.HealthyFat,
            UnitSymbol = healthFatToImport.UnitSymbol,
        };

        var createdHealthFat = context.Context!.Ingredients.SingleOrDefault(ingredient => ingredient.IngredientId == healthFatToImport.Id);

        createdHealthFat.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedHealthFat, options => options.Excluding(ingredient => ingredient.Id))
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Exception_On_Get_Get_Ingredients_Payload()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var familyId = Guid.NewGuid();

        var exception = new FamilyFileNotFoundException("file name");

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext();

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = exception.Code,
            ErrorMessage = exception.Message,
            FamilyId = familyId,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        context.Context!.Families.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Exception_On_Get_Get_Ingredients_To_Import()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var familyId = Guid.NewGuid();
        var ingredientInvalidType = IngredientTypeNames.Fruit;
        var payload = new byte[128];

        var exception = new WorksheetNotFoundException(ingredientInvalidType);

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        this.mediator
            .Setup(service => service.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParseIngredientsFromExcelFile request, CancellationToken _) =>
            {
                if (request.Payload != payload)
                {
                    throw new ArgumentException();
                }

                if (request.IngredientType == ingredientInvalidType)
                {
                    throw exception;
                }

                return new List<IngredientToImport>();
            });

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext();

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedResults = IngredientTypeNames.IngredientTypes
            .Select(ingredientType => new ImportFamilyDataResult
            {
                DataArea = ingredientType,
                Errors = ingredientType != ingredientInvalidType
                    ? new List<ImportFamilyDataError>()
                    : new List<ImportFamilyDataError>
                    {
                        new(exception.Code, exception.Message),
                    },
            }).ToList();

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = familyId,
            Results = expectedResults,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        context.Context!.Families.Should()
            .BeEmpty()
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Ingredient_Is_Invalid_Type()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var familyId = Guid.NewGuid();
        var payload = new byte[128];

        var familyFruit = new IngredientEntity
        {
            Exchanger = 1.2,
            FamilyId = familyId,
            IngredientId = Guid.NewGuid(),
            IsActive = true,
            IsSystem = true,
            Name = "fruit",
            Type = IngredientTypeNames.HealthyFat,
            UnitSymbol = "kg",
        };

        var family = new FamilyEntity
        {
            FamilyId = familyId,
        };

        var fruitToImport = new IngredientToImport(Exchanger: 1.5, familyFruit.IngredientId, "fruit 2", "g");

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        this.mediator
            .Setup(service => service.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParseIngredientsFromExcelFile request, CancellationToken _) =>
            {
                if (request.Payload != payload)
                {
                    throw new ArgumentException();
                }

                if (request.IngredientType == IngredientTypeNames.Fruit)
                {
                    return new List<IngredientToImport>
                    {
                        fruitToImport,
                    };
                }

                return new List<IngredientToImport>();
            });

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext(family, familyFruit);

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedException = new IngredientToImportInvalidTypeException(familyFruit.IngredientId, familyFruit.Type, IngredientTypeNames.Fruit);

        var expectedResults = IngredientTypeNames.IngredientTypes
            .Select(ingredientType => new ImportFamilyDataResult
            {
                DataArea = ingredientType,
                Errors = ingredientType != IngredientTypeNames.Fruit
                    ? new List<ImportFamilyDataError>()
                    : new List<ImportFamilyDataError>
                    {
                        new(expectedException.Code, expectedException.Message),
                    },
            }).ToList();

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = familyId,
            Results = expectedResults,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Publish_Notification_With_Error_When_Ingredient_Is_Not_System()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var familyId = Guid.NewGuid();
        var payload = new byte[128];

        var familyFruit = new IngredientEntity
        {
            Exchanger = 1.2,
            FamilyId = familyId,
            IngredientId = Guid.NewGuid(),
            IsActive = true,
            IsSystem = false,
            Name = "fruit",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var family = new FamilyEntity
        {
            FamilyId = familyId,
        };

        var fruitToImport = new IngredientToImport(Exchanger: 1.5, familyFruit.IngredientId, "fruit 2", "g");

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        this.mediator
            .Setup(service => service.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParseIngredientsFromExcelFile request, CancellationToken _) =>
            {
                if (request.Payload != payload)
                {
                    throw new ArgumentException();
                }

                if (request.IngredientType == IngredientTypeNames.Fruit)
                {
                    return new List<IngredientToImport>
                    {
                        fruitToImport,
                    };
                }

                return new List<IngredientToImport>();
            });

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext(family, familyFruit);

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedException = new IngredientToImportIsNotSystemException(familyFruit.IngredientId);

        var expectedResults = IngredientTypeNames.IngredientTypes
            .Select(ingredientType => new ImportFamilyDataResult
            {
                DataArea = ingredientType,
                Errors = ingredientType != IngredientTypeNames.Fruit
                    ? new List<ImportFamilyDataError>()
                    : new List<ImportFamilyDataError>
                    {
                        new(expectedException.Code, expectedException.Message),
                    },
            }).ToList();

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = familyId,
            Results = expectedResults,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Update_Family_And_Ingredients()
    {
        //GIVEN
        ImportedFamilyData? importedFamilyData = null;

        var emptyIngredientsHashString = new List<IngredientToImport>().GetHashString();
        var familyId = Guid.NewGuid();
        var payload = new byte[128];

        var familyFruit = new IngredientEntity
        {
            Exchanger = 1.2,
            FamilyId = familyId,
            IngredientId = Guid.NewGuid(),
            IsActive = true,
            IsSystem = true,
            Name = "fruit",
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = "kg",
        };

        var familyHealthFat = new IngredientEntity
        {
            Exchanger = 2.7,
            FamilyId = familyId,
            IngredientId = Guid.NewGuid(),
            IsActive = true,
            IsSystem = true,
            Name = "health fat",
            Type = IngredientTypeNames.HealthyFat,
            UnitSymbol = "g",
        };

        var family = new FamilyEntity
        {
            FamilyId = familyId,
            IngredientsVersion = IngredientTypeNames.IngredientTypes.ToDictionary(key => key, _ => string.Empty),
        };

        family.IngredientsVersion[IngredientTypeNames.Fruit] = new List<IngredientEntity>
        {
            familyFruit,
        }.GetHashString();

        family.IngredientsVersion[IngredientTypeNames.HealthyFat] = new List<IngredientEntity>
        {
            familyHealthFat,
        }.GetHashString();

        var fruitToImport = new IngredientToImport(Exchanger: 1.5, familyFruit.IngredientId, "fruit 2", "g");

        var fruitsToImportHashString = new List<IngredientToImport>
        {
            fruitToImport,
        }.GetHashString();

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(payload);

        this.mediator
            .Setup(service => service.Send(It.IsAny<ParseIngredientsFromExcelFile>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ParseIngredientsFromExcelFile request, CancellationToken _) =>
            {
                if (request.Payload != payload)
                {
                    throw new ArgumentException();
                }

                if (request.IngredientType == IngredientTypeNames.Fruit)
                {
                    return new List<IngredientToImport>
                    {
                        fruitToImport,
                    };
                }

                return new List<IngredientToImport>();
            });

        this.mediator
            .Setup(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()))
            .Callback<ImportedFamilyData, CancellationToken>((notification, _) => importedFamilyData = notification);

        using var context = new FakeDietMenuDbContext(family, familyFruit, familyHealthFat);

        var migrationService = new FamilyMigrationService(context.Context!, this.logger, this.mediator.Object);

        //WHEN
        await migrationService.ImportAsync(familyId, CancellationToken.None);

        //THEN
        this.mediator.Verify(service => service.Send(It.IsAny<GetFamilyIngredientsPayload>(), It.IsAny<CancellationToken>()), Times.Once);

        this.mediator.Verify(service => service.Publish(It.IsAny<ImportedFamilyData>(), It.IsAny<CancellationToken>()), Times.Once());

        var expectedResults = IngredientTypeNames.IngredientTypes
            .Select(ingredientType => new ImportFamilyDataResult
            {
                DataArea = ingredientType,
            }).ToList();

        var expectedNotification = new ImportedFamilyData
        {
            ErrorCode = null,
            ErrorMessage = null,
            FamilyId = familyId,
            Results = expectedResults,
        };

        importedFamilyData.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;

        var expectedFamily = new FamilyEntity
        {
            FamilyId = familyId,
            IngredientsVersion = IngredientTypeNames.IngredientTypes
                .ToDictionary(key => key, value =>
                {
                    if (value == IngredientTypeNames.Fruit)
                    {
                        return fruitsToImportHashString;
                    }

                    return emptyIngredientsHashString;
                }),
        };

        var createdFamily = context.Context!.Families.SingleOrDefault(familyEntity => familyEntity.FamilyId == familyId);

        createdFamily.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedFamily, options => options.Excluding(familyEntity => familyEntity.Id))
            ;

        var expectedFruit = new IngredientEntity
        {
            Exchanger = fruitToImport.Exchanger,
            FamilyId = familyId,
            IngredientId = fruitToImport.Id,
            IsActive = true,
            IsSystem = true,
            Name = fruitToImport.Name,
            Type = IngredientTypeNames.Fruit,
            UnitSymbol = fruitToImport.UnitSymbol,
        };

        var updatedFruit = context.Context!.Ingredients.SingleOrDefault(ingredient => ingredient.IngredientId == fruitToImport.Id);

        updatedFruit.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedFruit, options => options.Excluding(ingredient => ingredient.Id))
            ;

        var updatedHealthFat = context.Context!.Ingredients.SingleOrDefault(ingredient => ingredient.IngredientId == familyHealthFat.IngredientId);

        updatedHealthFat.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(familyHealthFat, options => options
                .Excluding(ingredient => ingredient.IsActive)
                .Excluding(ingredient => ingredient.Id))
            ;

        updatedHealthFat!.IsActive.Should()
            .BeFalse()
            ;
    }
}
