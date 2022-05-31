namespace SilentMike.DietMenu.Core.Application.Core.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Core.Commands;
using SilentMike.DietMenu.Core.Application.Core.Events;
using SilentMike.DietMenu.Core.Application.Core.Models;
using SilentMike.DietMenu.Core.Application.Core.Queries;
using SilentMike.DietMenu.Core.Application.Exceptions.Core;
using SilentMike.DietMenu.Core.Domain.Repositories;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class ImportCoreDataHandler : IRequestHandler<ImportCoreData>
{
    private readonly ICoreRepository coreRepository;
    private readonly ILogger<ImportCoreDataHandler> logger;
    private readonly IMediator mediator;

    public ImportCoreDataHandler(ICoreRepository coreRepository, ILogger<ImportCoreDataHandler> logger, IMediator mediator)
        => (this.coreRepository, this.logger, this.mediator) = (coreRepository, logger, mediator);

    public async Task<Unit> Handle(ImportCoreData request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to import core data");

        try
        {
            var payloadsToImport = await this.GetPayloadsToImport(cancellationToken);

            var query = new GetCoreDataToImport
            {
                IngredientsPayload = payloadsToImport.IngredientsPayload,
            };

            var dataToImport = await this.mediator.Send(query, cancellationToken);

            if (dataToImport.Exceptions.Any())
            {
                throw new CoreDataImportException(dataToImport.Exceptions);
            }

            if (!request.ValidationOnly)
            {
                this.coreRepository.SaveIngredientTypes(dataToImport.IngredientTypes);
                this.coreRepository.SaveIngredients(dataToImport.Ingredients);
                this.coreRepository.SaveMealTypes(dataToImport.MealTypes);
                this.coreRepository.SaveCore(dataToImport.Core);
            }

            var notification = new ImportedCoreData
            {
                IsSuccess = true,
            };

            await this.mediator.Publish(notification, cancellationToken);

            return await Task.FromResult(Unit.Value);
        }
        catch (Exception exception)
        {
            await this.HandleException(exception, cancellationToken);

            throw;
        }
    }

    private async Task<CoreDataPayloadsToImport> GetPayloadsToImport(CancellationToken cancellationToken)
    {
        var query = new GetCoreDataPayloadsToImport();

        var payloadsToImport = await this.mediator.Send(query, cancellationToken);

        return payloadsToImport;
    }

    private async Task HandleException(Exception exception, CancellationToken cancellationToken)
    {
        var errors = exception switch
        {
            CoreDataImportException importException => GetErrorFromImportException(importException),
            ApplicationException applicationException => GetErrorsFromApplicationException(applicationException),
            _ => GetErrorsFromException(exception),
        };

        var notification = new ImportedCoreData
        {
            Errors = errors.AsReadOnly(),
            IsSuccess = false,
        };

        await this.mediator.Publish(notification, cancellationToken);
    }

    private static List<CoreDataImportError> GetErrorsFromApplicationException(ApplicationException applicationException)
    {
        var importErrors = new List<CoreDataImportError>();

        var importError = new CoreDataImportError
        {
            DataArea = "Core",
            Errors = new Dictionary<string, string[]>
            {
                { applicationException.Code, new[] { applicationException.Message } },
            },
        };

        importErrors.Add(importError);

        return importErrors;
    }

    private static List<CoreDataImportError> GetErrorsFromException(Exception exception)
    {
        var importErrors = new List<CoreDataImportError>();

        var importError = new CoreDataImportError
        {
            DataArea = "Core",
            Errors = new Dictionary<string, string[]>
            {
                { ErrorCodes.UNKNOWN_ERROR, new[] { exception.Message } },
            },
        };

        importErrors.Add(importError);

        return importErrors;
    }

    private static List<CoreDataImportError> GetErrorFromImportException(CoreDataImportException importException)
    {
        var importErrors = new List<CoreDataImportError>();

        foreach (var (dataArea, exceptions) in importException.Exceptions)
        {
            var errors = exceptions
                .GroupBy(exception => exception.Code, exception => exception.Message)
                .ToDictionary(exceptionsGroup => exceptionsGroup.Key, exceptionsGroup => exceptionsGroup.ToArray());

            var importError = new CoreDataImportError
            {
                DataArea = dataArea,
                Errors = errors,
            };

            importErrors.Add(importError);
        }

        return importErrors;
    }
}
