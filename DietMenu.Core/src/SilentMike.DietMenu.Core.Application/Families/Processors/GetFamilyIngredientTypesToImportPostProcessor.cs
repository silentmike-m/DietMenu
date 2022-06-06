namespace SilentMike.DietMenu.Core.Application.Families.Processors;

using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Domain.Common.Constants;
using SilentMike.DietMenu.Core.Domain.Entities;
using ApplicationException = SilentMike.DietMenu.Core.Application.Common.ApplicationException;

internal sealed class GetFamilyIngredientTypesToImportPostProcessor<TRequest, TResponse> : IGetFamilyDataToImportPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IGetFamilyDataToImport
    where TResponse : IFamilyDataToImport
{
    private readonly string dataName = DataNames.IngredientTypes;

    private readonly ILogger<GetFamilyIngredientTypesToImportPostProcessor<TRequest, TResponse>> logger;

    public GetFamilyIngredientTypesToImportPostProcessor(ILogger<GetFamilyIngredientTypesToImportPostProcessor<TRequest, TResponse>> logger)
        => this.logger = logger;

    public async Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to import family {DataName}", this.dataName);

        try
        {
            if (string.Equals(request.Family[this.dataName], request.Core[this.dataName], StringComparison.InvariantCultureIgnoreCase))
            {
                this.logger.LogInformation("Portfolio {DataName} is up to date", this.dataName);
            }
            else
            {
                Import(request, response);

                request.Family[this.dataName] = request.Core[this.dataName];
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

    private static void Import(TRequest request, TResponse response)
    {
        foreach (var coreIngredientType in request.CoreIngredientTypes)
        {
            var familyIngredientType = response.IngredientTypes
                    .SingleOrDefault(type => type.InternalName == coreIngredientType.InternalName)
                ;

            if (familyIngredientType is null)
            {
                familyIngredientType = new IngredientTypeEntity(Guid.NewGuid())
                {
                    FamilyId = request.Family.Id,
                    InternalName = coreIngredientType.InternalName,
                    IsActive = true,
                    Name = coreIngredientType.Name,
                };

                response.IngredientTypes.Add(familyIngredientType);
            }
            else
            {
                familyIngredientType.IsActive = true;
                familyIngredientType.Name = coreIngredientType.Name;
            }
        }
    }
}
