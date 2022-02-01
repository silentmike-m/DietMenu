namespace SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class ImportIngredientTypesHandler : IRequestHandler<ImportIngredientTypes>
{
    private readonly ILogger<ImportIngredientTypesHandler> logger;
    private readonly IIngredientTypeRepository repository;

    public ImportIngredientTypesHandler(ILogger<ImportIngredientTypesHandler> logger, IIngredientTypeRepository repository)
        => (this.logger, this.repository) = (logger, repository);

    public async Task<Unit> Handle(ImportIngredientTypes request, CancellationToken cancellationToken)
    {
        using var scopeLogger = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to import ingredient types");

        var ingredientTypes = new List<IngredientTypeEntity>();

        Add(ingredientTypes, request.FamilyId, "ComplexCarbohydrate", "Węglowodan złożony");
        Add(ingredientTypes, request.FamilyId, "Fruit", "Owoc");
        Add(ingredientTypes, request.FamilyId, "HealthyFat", "Zdrowy tłuszcz");
        Add(ingredientTypes, request.FamilyId, "HighFatProtein", "Białko wysokotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "LowFatProtein", "Białko niskotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "MediumFatProtein", "Białko średniotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "Other", "Inne");

        await this.repository.Save(ingredientTypes, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private static void Add(ICollection<IngredientTypeEntity> self, Guid familyId, string internalName, string name)
    {
        var type = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = familyId,
            InternalName = internalName,
            IsSystem = true,
            Name = name,
        };

        self.Add(type);
    }
}
