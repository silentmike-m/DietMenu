namespace SilentMike.DietMenu.Core.Application.Core.PostProcessors;

using SilentMike.DietMenu.Core.Application.Core.Interfaces;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class GetCoreMealTypesToImportPostProcessor<TRequest, TResponse> : IGetCoreDataToImportPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetCoreDataToImport
    where TResponse : ICoreDataToImport
{
    private const string DATA_VERSION = "INIT";

    private readonly string dataName = DataNames.MealTypes;

    private readonly ILogger<GetCoreMealTypesToImportPostProcessor<TRequest, TResponse>> logger;

    public GetCoreMealTypesToImportPostProcessor(ILogger<GetCoreMealTypesToImportPostProcessor<TRequest, TResponse>> logger)
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
                Import(response);

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

    private static void Import(TResponse response)
    {
        Add(response.MealTypes, MealTypeNames.FirstBreakfast, "I śniadanie", 1);
        Add(response.MealTypes, MealTypeNames.SecondBreakfast, "II śniadanie", 2);
        Add(response.MealTypes, MealTypeNames.Snack, "Przekąska", 3);
        Add(response.MealTypes, MealTypeNames.Dinner, "Obiad", 4);
        Add(response.MealTypes, MealTypeNames.Dessert, "Deser", 5);
        Add(response.MealTypes, MealTypeNames.Supper, "Kolacja", 6);
    }

    private static void Add(ICollection<CoreMealTypeEntity> self, string internalName, string name, int order)
    {
        var mealType = new CoreMealTypeEntity(Guid.NewGuid())
        {
            InternalName = internalName,
            Name = name,
            Order = order,
        };

        self.Add(mealType);
    }
}
