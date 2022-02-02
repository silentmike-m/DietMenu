namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class ImportIngredientsHandler : IRequestHandler<ImportIngredients>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<ImportIngredientsHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientRepository ingredientRepository;
    private readonly IIngredientTypeRepository ingredientTypeRepository;

    public ImportIngredientsHandler(
        IFamilyRepository familyRepository,
        ILogger<ImportIngredientsHandler> logger,
        IMediator mediator,
        IIngredientRepository ingredientRepository,
        IIngredientTypeRepository ingredientTypeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.ingredientRepository = ingredientRepository;
        this.ingredientTypeRepository = ingredientTypeRepository;
    }

    public async Task<Unit> Handle(ImportIngredients request, CancellationToken cancellationToken)
    {
        using var scopeLogger = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to import ingredient types");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var ingredientTypes = await this.ingredientTypeRepository.GetByFamilyId(request.FamilyId, cancellationToken);

        var ingredients = new List<IngredientEntity>();

        foreach (var ingredientType in ingredientTypes)
        {
            var query = new ParseIngredientsFromExcelFile
            {
                FamilyId = request.FamilyId,
                Payload = request.Payload,
                TypeId = ingredientType.Id,
                TypeInternalName = ingredientType.InternalName,
            };

            var parsedIngredients = await this.mediator.Send(query, cancellationToken);

            ingredients.AddRange(parsedIngredients);
        }

        await this.ingredientRepository.Save(ingredients, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
