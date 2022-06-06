namespace SilentMike.DietMenu.Core.Application.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Core;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Models;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class GetCoreIngredientsToImportPostProcessor<TRequest, TResponse> : IGetCoreDataToImportPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetCoreDataToImport
    where TResponse : ICoreDataToImport
{
    private readonly string defaultDataName = DataNames.Ingredients;

    private readonly ILogger<GetCoreIngredientsToImportPostProcessor<TRequest, TResponse>> logger;
    private readonly IMediator mediator;

    public GetCoreIngredientsToImportPostProcessor(
        ILogger<GetCoreIngredientsToImportPostProcessor<TRequest, TResponse>> logger,
        IMediator mediator)
    {

        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to import core {DataName}", this.defaultDataName);

        try
        {
            if (request.IngredientsPayload.Length == 0)
            {
                throw new CoreDataImportEmptyPayloadException();
            }

            foreach (var ingredientType in response.IngredientTypes)
            {
                await this.ImportIngredient(request, response, ingredientType, cancellationToken);
            }
        }
        catch (ApplicationException applicationException)
        {
            response.AddException(this.defaultDataName, applicationException);
        }
        catch (Exception exception)
        {
            response.AddException(this.defaultDataName, new UnknownErrorException(exception.Message));
        }

        await Task.CompletedTask;
    }

    private async Task ImportIngredient(TRequest request, TResponse response, CoreIngredientTypeEntity ingredientType, CancellationToken cancellationToken)
    {
        try
        {
            var query = new ParseIngredientsFromExcelFile
            {
                Payload = request.IngredientsPayload,
                TypeInternalName = ingredientType.InternalName,
            };

            var excelData = await this.mediator.Send(query, cancellationToken);

            var excelDataHashString = excelData.GetHashString();

            if (string.Equals(response.Core[ingredientType.InternalName], excelDataHashString, StringComparison.InvariantCultureIgnoreCase))
            {
                this.logger.LogInformation("Core ingredients {TypeName} are up to date", ingredientType.InternalName);
            }
            else
            {
                var coreIngredients = response.Ingredients
                    .Where(ingredient => ingredient.TypeId == ingredientType.Id)
                    .ToList();

                var ingredientsToImport = MapIngredients(excelData, ingredientType, response);

                DeactivateIngredients(coreIngredients, ingredientsToImport);

                foreach (var ingredientToImport in ingredientsToImport)
                {
                    var ingredient = coreIngredients
                        .SingleOrDefault(ingredient => string.Equals(ingredient.InternalName, ingredientToImport.InternalName,
                            StringComparison.InvariantCultureIgnoreCase));

                    if (ingredient is null)
                    {
                        response.Ingredients.Add(ingredientToImport);
                    }
                    else
                    {
                        UpdateIngredient(ingredient, ingredientToImport);
                    }
                }

                response.Core[ingredientType.InternalName] = excelDataHashString;
            }
        }
        catch (ApplicationException applicationException)
        {
            response.AddException(ingredientType.InternalName, applicationException);
        }
        catch (Exception exception)
        {
            response.AddException(ingredientType.InternalName, new UnknownErrorException(exception.Message));
        }

        await Task.CompletedTask;
    }

    private static IReadOnlyList<CoreIngredientEntity> MapIngredients(IReadOnlyList<IngredientToImport> excelData, CoreIngredientTypeEntity ingredientType, TResponse response)
    {
        var ingredients = new List<CoreIngredientEntity>();

        foreach (var ingredientToImport in excelData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ingredientToImport.InternalName))
                {
                    throw new IngredientEmptyInternalNameException();
                }

                var duplicatedInternalNames = excelData
                    .Where(ingredient => string.Equals(ingredient.InternalName, ingredientToImport.InternalName, StringComparison.InvariantCultureIgnoreCase));

                var isInternalNameDuplicated = duplicatedInternalNames.Count() > 1;

                if (isInternalNameDuplicated)
                {
                    throw new IngredientDuplicatedInternalNameException(ingredientToImport.InternalName);
                }

                var ingredient = new CoreIngredientEntity(Guid.NewGuid())
                {
                    Exchanger = ingredientToImport.Exchanger,
                    InternalName = ingredientToImport.InternalName,
                    Name = ingredientToImport.Name,
                    Type = ingredientType,
                    TypeId = ingredientType.Id,
                    UnitSymbol = ingredientToImport.UnitSymbol,
                };

                ingredients.Add(ingredient);
            }
            catch (ApplicationException applicationException)
            {
                response.AddException(ingredientType.InternalName, applicationException);
            }
        }

        return ingredients;
    }

    private static void DeactivateIngredients(IEnumerable<CoreIngredientEntity> coreIngredients, IReadOnlyList<CoreIngredientEntity> excelIngredients)
    {
        foreach (var coreIngredient in coreIngredients)
        {
            var excelIngredient = excelIngredients
                .SingleOrDefault(ingredient => string.Equals(ingredient.InternalName, coreIngredient.InternalName, StringComparison.InvariantCultureIgnoreCase));

            if (excelIngredient is null)
            {
                coreIngredient.IsActive = false;
            }
        }
    }

    private static void UpdateIngredient(CoreIngredientEntity ingredient, CoreIngredientEntity ingredientToImport)
    {
        ingredient.Exchanger = ingredientToImport.Exchanger;
        ingredient.IsActive = true;
        ingredient.Name = ingredientToImport.Name;
        ingredient.UnitSymbol = ingredientToImport.UnitSymbol;
    }
}
