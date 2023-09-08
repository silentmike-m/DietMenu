namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Extensions;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Common;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Models;
using SilentMike.DietMenu.Core.Infrastructure.Common;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;
using SilentMike.DietMenu.Core.Infrastructure.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Infrastructure.Families.Interfaces;

internal sealed class FamilyMigrationService : IFamilyMigrationService
{
    private readonly IDietMenuDbContext context;
    private readonly ILogger<FamilyMigrationService> logger;
    private readonly IMediator mediator;

    public FamilyMigrationService(IDietMenuDbContext context, ILogger<FamilyMigrationService> logger, IMediator mediator)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task ImportAsync(Guid familyId, CancellationToken cancellationToken = default)
    {
        using var loggerScope = this.logger.BeginPropertyScope("FamilyId", familyId);

        this.logger.LogInformation("Try to import family data");

        var importResults = new List<ImportFamilyDataResult>();

        try
        {
            var family = await this.GetOrCreateFamily(familyId, cancellationToken);

            var ingredientsPayload = await this.GetIngredientsPayload(cancellationToken);

            foreach (var ingredientType in IngredientTypeNames.IngredientTypes)
            {
                var importResult = await this.ImportIngredientType(family, ingredientType, ingredientsPayload, cancellationToken);

                importResults.Add(importResult);
            }

            var isAnyException = importResults.Exists(result => result.Errors.Any());

            if (isAnyException is false)
            {
                _ = await this.context.SaveChangesAsync(cancellationToken);
            }

            await this.PublishNotification(error: null, familyId, importResults, cancellationToken);
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            var error = MapException(exception);

            await this.PublishNotification(error, familyId, importResults, cancellationToken);
        }
    }

    private IEnumerable<ImportFamilyDataError> DeleteIngredients(Guid familyId, string ingredientType, IReadOnlyList<IngredientToImport> ingredientsToImport)
    {
        var errors = new List<ImportFamilyDataError>();

        try
        {
            var systemIngredients = this.context.Ingredients
                .Where(ingredient => ingredient.FamilyId == familyId)
                .Where(ingredient => ingredient.IsSystem)
                .Where(ingredient => ingredient.IsActive)
                .Where(ingredient => ingredient.Type == ingredientType);

            foreach (var systemIngredient in systemIngredients)
            {
                var ingredientToImport = ingredientsToImport.SingleOrDefault(ingredient => ingredient.Id == systemIngredient.IngredientId);

                if (ingredientToImport is null)
                {
                    systemIngredient.IsActive = false;
                }
            }
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            var error = MapException(exception);

            errors.Add(error);
        }

        return errors;
    }

    private async Task<byte[]> GetIngredientsPayload(CancellationToken cancellationToken)
    {
        var request = new GetFamilyIngredientsPayload();

        var result = await this.mediator.Send(request, cancellationToken);

        return result;
    }

    private async Task<IReadOnlyList<IngredientToImport>> GetIngredientsToImport(string ingredientType, byte[] payload, CancellationToken cancellationToken)
    {
        var request = new ParseIngredientsFromExcelFile(ingredientType, payload);

        var result = await this.mediator.Send(request, cancellationToken);

        return result;
    }

    private async Task<FamilyEntity> GetOrCreateFamily(Guid familyId, CancellationToken cancellationToken)
    {
        var family = await this.context.Families.SingleOrDefaultAsync(family => family.FamilyId == familyId, cancellationToken);

        if (family is not null)
        {
            return family;
        }

        this.logger.LogInformation("Family has not been found, a new one will be created");

        family = new FamilyEntity
        {
            FamilyId = familyId,
        };

        await this.context.Families.AddAsync(family, cancellationToken);

        return family;
    }

    private async Task<List<ImportFamilyDataError>> ImportIngredients(Guid familyId, string ingredientType, IEnumerable<IngredientToImport> ingredientsToImport, CancellationToken cancellationToken)
    {
        var errors = new List<ImportFamilyDataError>();

        foreach (var ingredientToImport in ingredientsToImport)
        {
            try
            {
                ValidateIngredientToImport(familyId, ingredientToImport, ingredientType);

                var ingredient = await this.context.Ingredients
                    .Where(ingredient => ingredient.FamilyId == familyId)
                    .SingleOrDefaultAsync(ingredient => ingredient.IngredientId == ingredientToImport.Id, cancellationToken);

                if (ingredient is null)
                {
                    ingredient = new IngredientEntity
                    {
                        Exchanger = ingredientToImport.Exchanger,
                        FamilyId = familyId,
                        IngredientId = ingredientToImport.Id,
                        IsActive = true,
                        IsSystem = true,
                        Name = ingredientToImport.Name,
                        Type = ingredientType,
                        UnitSymbol = ingredientToImport.UnitSymbol,
                    };

                    await this.context.Ingredients.AddAsync(ingredient, cancellationToken);
                }
                else
                {
                    if (ingredient.Type != ingredientType)
                    {
                        throw new IngredientToImportInvalidTypeException(ingredient.IngredientId, ingredient.Type, ingredientType);
                    }

                    if (ingredient.IsSystem is false)
                    {
                        throw new IngredientToImportIsNotSystemException(ingredient.IngredientId);
                    }

                    ingredient.IsActive = true;
                    ingredient.Name = ingredientToImport.Name;
                    ingredient.Exchanger = ingredientToImport.Exchanger;
                    ingredient.UnitSymbol = ingredientToImport.UnitSymbol;
                }
            }
            catch (Exception exception)
            {
                this.logger.LogError(exception, "{Message}", exception.Message);

                var error = MapException(exception);

                errors.Add(error);
            }
        }

        return errors;
    }

    private async Task<ImportFamilyDataResult> ImportIngredientType(FamilyEntity family, string ingredientType, byte[] payload, CancellationToken cancellationToken)
    {
        var errors = new List<ImportFamilyDataError>();

        try
        {
            var ingredientsToImport = await this.GetIngredientsToImport(ingredientType, payload, cancellationToken);

            var hashString = ingredientsToImport.GetHashString();

            if (family.IngredientsVersion.TryGetValue(ingredientType, out var familyHashString) is false)
            {
                familyHashString = string.Empty;

                family.IngredientsVersion.Add(ingredientType, familyHashString);
            }

            var isHashStringEqual = hashString.IsInvariantCultureIgnoreCaseEquals(familyHashString);

            if (isHashStringEqual is false)
            {
                var deleteErrors = this.DeleteIngredients(family.FamilyId, ingredientType, ingredientsToImport);
                errors.AddRange(deleteErrors);

                var importErrors = await this.ImportIngredients(family.FamilyId, ingredientType, ingredientsToImport, cancellationToken);
                errors.AddRange(importErrors);

                family.IngredientsVersion[ingredientType] = hashString;
            }
            else
            {
                this.logger.LogDebug("Hash string for {IngredientType} is equal", ingredientType);
            }
        }
        catch (Exception exception)
        {
            this.logger.LogError(exception, "{Message}", exception.Message);

            var error = MapException(exception);

            errors.Add(error);
        }

        var result = new ImportFamilyDataResult
        {
            DataArea = ingredientType,
            Errors = errors,
        };

        return result;
    }

    private async Task PublishNotification(ImportFamilyDataError? error, Guid familyId, IReadOnlyList<ImportFamilyDataResult> results, CancellationToken cancellationToken)
    {
        var notification = new ImportedFamilyData
        {
            ErrorCode = error?.Code,
            ErrorMessage = error?.Message,
            FamilyId = familyId,
            Results = results,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }

    private static ImportFamilyDataError MapException(Exception exception)
    {
        var (code, message) = exception switch
        {
            ApplicationException domainException => (domainException.Code, domainException.Message),
            DomainException domainException => (domainException.Code, domainException.Message),
            InfrastructureException domainException => (domainException.Code, domainException.Message),
            _ => (string.Empty, exception.Message),
        };

        return new ImportFamilyDataError(code, message);
    }

    private static void ValidateIngredientToImport(Guid familyId, IngredientToImport ingredientToImport, string ingredientType)
    {
        _ = new Ingredient(ingredientToImport.Exchanger, familyId, ingredientToImport.Name, ingredientType, ingredientToImport.UnitSymbol);
    }
}
