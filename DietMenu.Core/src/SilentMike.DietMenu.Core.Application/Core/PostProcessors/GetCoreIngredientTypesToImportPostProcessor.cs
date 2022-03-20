namespace SilentMike.DietMenu.Core.Application.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class GetCoreIngredientTypesToImportPostProcessor<TRequest, TResponse> : IGetCoreDataToImportPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetCoreDataToImport
    where TResponse : ICoreDataToImport
{
    private const string DATA_VERSION = "INIT";

    private readonly string dataName = DataNames.IngredientTypes;

    private readonly ILogger<GetCoreIngredientTypesToImportPostProcessor<TRequest, TResponse>> logger;

    private readonly Dictionary<string, string> ingredientTypesMappings = new()
    {
        { IngredientTypeNames.ComplexCarbohydrate, "Węglowodan złożony" },
        { IngredientTypeNames.Fruit, "Owoc" },
        { IngredientTypeNames.HealthyFat, "Zdrowy tłuszcz" },
        { IngredientTypeNames.HighFatProtein, "Białko wysokotłuszczowe" },
        { IngredientTypeNames.LowFatProtein, "Białko niskotłuszczowe" },
        { IngredientTypeNames.MediumFatProtein, "Białko średniotłuszczowe" },
        { IngredientTypeNames.Other, "Inne" },
    };

    public GetCoreIngredientTypesToImportPostProcessor(ILogger<GetCoreIngredientTypesToImportPostProcessor<TRequest, TResponse>> logger)
        => this.logger = logger;

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to import core {DataName}", this.dataName);

        try
        {
            if (string.Equals(response.Core[this.dataName], DATA_VERSION, StringComparison.InvariantCultureIgnoreCase))
            {
                this.logger.LogInformation("Core {DataName} is up to date", this.dataName);
            }
            else
            {
                this.Import(response);

                response.Core[this.dataName] = DATA_VERSION;
            }
        }
        catch (ApplicationException applicationException)
        {
            response.AddException(this.dataName, applicationException);
        }
        catch (Exception exception)
        {
            response.AddException(this.dataName, new UnknownErrorException(exception.Message));
        }

        await Task.CompletedTask;
    }

    private void Import(TResponse response)
    {
        foreach (var (internalName, name) in ingredientTypesMappings)
        {
            var type = new CoreIngredientTypeEntity(Guid.NewGuid())
            {
                InternalName = internalName,
                Name = name,
            };

            response.IngredientTypes.Add(type);
        }
    }
}
