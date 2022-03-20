namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Jobs;

using System.ComponentModel;
using global::Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Core;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class ImportFamilyData
{
    private readonly DietMenuDbContext context;
    private readonly ILogger<ImportFamilyData> logger;
    private readonly IMediator mediator;

    public ImportFamilyData(DietMenuDbContext context, ILogger<ImportFamilyData> logger, IMediator mediator)
        => (this.context, this.logger, this.mediator) = (context, logger, mediator);

    [DisplayName("Import data for family with id {0}")]
    [AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid familyId)
    {
        this.logger.LogInformation("Try to import family data");

        try
        {
            var family = this.context.Families.SingleOrDefault(family => family.Id == familyId);

            if (family is null)
            {
                throw new FamilyNotFoundException(familyId);
            }

            var familyDataToImport = await this.GetFamilyDataToImport(family);

            if (familyDataToImport.Exceptions.Any())
            {
                throw new FamilyDataImportException(familyDataToImport.Exceptions);
            }

            await this.context.Save(familyDataToImport.IngredientTypes.AsEnumerable(), CancellationToken.None);
            await this.context.Save(familyDataToImport.Ingredients.AsEnumerable(), CancellationToken.None);
            await this.context.Save(familyDataToImport.MealTypes.AsEnumerable(), CancellationToken.None);
            await this.context.Save(family, CancellationToken.None);

            var notification = new ImportedFamilyData
            {
                FamilyId = familyId,
                IsSuccess = true,
            };

            await this.mediator.Publish(notification, CancellationToken.None);
        }
        catch (Exception exception)
        {
            await this.HandleException(familyId, exception);

            throw;
        }
    }

    private async Task<FamilyDataToImport> GetFamilyDataToImport(FamilyEntity family)
    {
        var core = this.context.Core.SingleOrDefault();

        if (core is null)
        {
            throw new CoreNotFoundException();
        }

        var coreIngredientTypes = this.context.CoreIngredientTypes
            .ToList();

        var coreIngredients = this.context.CoreIngredients
            .Where(ingredient => ingredient.IsActive)
            .ToList();

        var coreMealTypes = this.context.CoreMealTypes
            .ToList();

        var query = new GetFamilyDataToImport
        {
            Core = core,
            CoreIngredientTypes = coreIngredientTypes,
            CoreIngredients = coreIngredients,
            CoreMealTypes = coreMealTypes,
            Family = family,
        };

        var familyDataToImport = await this.mediator.Send(query, CancellationToken.None);

        return familyDataToImport;
    }

    private async Task HandleException(Guid familyId, Exception exception)
    {
        var errors = exception switch
        {
            FamilyDataImportException importException => GetErrorFromImportException(importException),
            ApplicationException applicationException => GetErrorsFromApplicationException(applicationException),
            _ => GetErrorsFromException(exception),
        };

        var notification = new ImportedFamilyData
        {
            Errors = errors.AsReadOnly(),
            FamilyId = familyId,
            IsSuccess = false,
        };

        await this.mediator.Publish(notification, CancellationToken.None);
    }

    private static List<FamilyDataImportError> GetErrorsFromApplicationException(ApplicationException applicationException)
    {
        var importErrors = new List<FamilyDataImportError>();

        var importError = new FamilyDataImportError
        {
            DataArea = "Family",
            Errors = new Dictionary<string, string[]>
            {
                { applicationException.Code, new[] { applicationException.Message } },
            },
        };

        importErrors.Add(importError);

        return importErrors;
    }

    private static List<FamilyDataImportError> GetErrorsFromException(Exception exception)
    {
        var importErrors = new List<FamilyDataImportError>();

        var importError = new FamilyDataImportError
        {
            DataArea = "Family",
            Errors = new Dictionary<string, string[]>
            {
                { ErrorCodes.UNKNOWN_ERROR, new[] { exception.Message } },
            },
        };

        importErrors.Add(importError);

        return importErrors;
    }

    private static List<FamilyDataImportError> GetErrorFromImportException(FamilyDataImportException importException)
    {
        var importErrors = new List<FamilyDataImportError>();

        foreach (var (dataArea, exceptions) in importException.Exceptions)
        {
            var errors = exceptions
                .GroupBy(exception => exception.Code, exception => exception.Message)
                .ToDictionary(exceptionsGroup => exceptionsGroup.Key, exceptionsGroup => exceptionsGroup.ToArray());

            var importError = new FamilyDataImportError
            {
                DataArea = dataArea,
                Errors = errors,
            };

            importErrors.Add(importError);
        }

        return importErrors;
    }
}
