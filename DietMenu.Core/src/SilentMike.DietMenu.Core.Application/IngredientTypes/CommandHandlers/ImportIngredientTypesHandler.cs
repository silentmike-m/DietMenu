namespace SilentMike.DietMenu.Core.Application.IngredientTypes.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class ImportIngredientTypesHandler : IRequestHandler<ImportIngredientTypes>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<ImportIngredientTypesHandler> logger;
    private readonly IIngredientTypeRepository typeRepository;

    public ImportIngredientTypesHandler(
        IFamilyRepository familyRepository,
        ILogger<ImportIngredientTypesHandler> logger,
        IIngredientTypeRepository typesRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.typeRepository = typesRepository;
    }

    public async Task<Unit> Handle(ImportIngredientTypes request, CancellationToken cancellationToken)
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

        var ingredientTypes = new List<IngredientTypeEntity>();

        Add(ingredientTypes, request.FamilyId, "ComplexCarbohydrate", "Węglowodan złożony");
        Add(ingredientTypes, request.FamilyId, "Fruit", "Owoc");
        Add(ingredientTypes, request.FamilyId, "HealthyFat", "Zdrowy tłuszcz");
        Add(ingredientTypes, request.FamilyId, "HighFatProtein", "Białko wysokotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "LowFatProtein", "Białko niskotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "MediumFatProtein", "Białko średniotłuszczowe");
        Add(ingredientTypes, request.FamilyId, "Other", "Inne");

        await this.typeRepository.Save(ingredientTypes, cancellationToken);

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
