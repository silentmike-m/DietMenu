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

        var mealTypeFirstBreakfast = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "FirstBreakfast",
            Name = "I śniadanie",
        };
        mealTypes.Add(mealTypeFirstBreakfast);

        var mealTypeSecondBreakfast = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "SecondBreakfast",
            Name = "II śniadanie",
        };
        mealTypes.Add(mealTypeSecondBreakfast);

        var mealTypeDinner = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "Dinner",
            Name = "Obiad",
        };
        mealTypes.Add(mealTypeDinner);

        var mealTypeSupper = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "Supper",
            Name = "Kolacja",
        };
        mealTypes.Add(mealTypeSupper);

        var mealTypeSnack = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "Snack",
            Name = "Przekąska",
        };
        mealTypes.Add(mealTypeSnack);

        var mealTypeDessert = new MealTypeEntity(Guid.NewGuid())
        {
            FamilyId = request.FamilyId,
            InternalName = "Dessert",
            Name = "Deser",
        };
        mealTypes.Add(mealTypeDessert);

        await this.repository.Save(mealTypes, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
