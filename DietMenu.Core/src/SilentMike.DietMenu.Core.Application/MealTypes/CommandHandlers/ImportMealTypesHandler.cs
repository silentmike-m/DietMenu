namespace SilentMike.DietMenu.Core.Application.MealTypes.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class ImportMealTypesHandler : IRequestHandler<ImportMealTypes>
{
    private readonly ILogger<ImportMealTypesHandler> logger;
    private readonly IMealTypeRepository repository;

    public ImportMealTypesHandler(ILogger<ImportMealTypesHandler> logger, IMealTypeRepository repository)
        => (this.logger, this.repository) = (logger, repository);

    public async Task<Unit> Handle(ImportMealTypes request, CancellationToken cancellationToken)
    {
        using var scopeLogger = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId)
        );

        this.logger.LogInformation("Try to import meal types");

        var mealTypes = new List<MealTypeEntity>();

        Add(mealTypes, request.FamilyId, "FirstBreakfast", "I śniadanie", 1);
        Add(mealTypes, request.FamilyId, "SecondBreakfast", "II śniadanie", 2);
        Add(mealTypes, request.FamilyId, "Snack", "Przekąska", 3);
        Add(mealTypes, request.FamilyId, "Dinner", "Obiad", 4);
        Add(mealTypes, request.FamilyId, "Dessert", "Deser", 5);
        Add(mealTypes, request.FamilyId, "Supper", "Kolacja", 6);

        await this.repository.Save(mealTypes, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private static void Add(ICollection<MealTypeEntity> self, Guid familyId, string internalName, string name, int order)
    {
        var mealType = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = familyId,
            InternalName = internalName,
            Name = name,
            Order = order,
        };

        self.Add(mealType);
    }
}
