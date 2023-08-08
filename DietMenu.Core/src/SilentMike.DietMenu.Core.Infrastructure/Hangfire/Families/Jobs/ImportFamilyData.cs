namespace SilentMike.DietMenu.Core.Infrastructure.Hangfire.Families.Jobs;

using System.ComponentModel;
using global::Hangfire;
using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Families.Events;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Entities;

internal sealed class ImportFamilyData
{
    private readonly DietMenuDbContext context;
    private readonly ILogger<ImportFamilyData> logger;
    private readonly IMediator mediator;

    public ImportFamilyData(DietMenuDbContext context, ILogger<ImportFamilyData> logger, IMediator mediator)
        => (this.context, this.logger, this.mediator) = (context, logger, mediator);

    [DisplayName("Import data for family with id {0}"), AutomaticRetry(Attempts = 0)]
    public async Task Run(Guid familyId)
    {
        this.logger.LogInformation("Try to import family data");

        try
        {
            var family = this.context.Families
                .AsNoTracking()
                .SingleOrDefault(family => family.InternalId == familyId);

            if (family is null)
            {
                family = CreateFamily(familyId);

                this.context.Add(family);
            }

            var familyDataToImport = await this.GetFamilyDataToImport(familyId);
            //
            // if (familyDataToImport.Exceptions.Any())
            // {
            //     throw new FamilyDataImportException(familyDataToImport.Exceptions);
            // }

            // this.context.Upsert<IngredientTypeEntity>(familyDataToImport.IngredientTypes);
            // this.context.Upsert<IngredientEntity>(familyDataToImport.Ingredients);
            // this.context.Upsert<MealTypeEntity>(familyDataToImport.MealTypes);
            // this.context.Upsert(family);

            this.context.SaveChanges();

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

    private async Task<FamilyDataToImport> GetFamilyDataToImport(Guid familyId)
    {
        var query = new GetFamilyDataToImport
        {
            FamilyId = familyId,
        };

        var result = await this.mediator.Send(query, CancellationToken.None);

        return result;
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

    private static FamilyEntity CreateFamily(Guid familyId)
    {
        var family = new FamilyEntity
        {
            InternalId = familyId,
        };

        return family;
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
}
