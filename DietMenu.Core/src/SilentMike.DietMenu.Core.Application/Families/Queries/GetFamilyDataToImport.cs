namespace SilentMike.DietMenu.Core.Application.Families.Queries;

using SilentMike.DietMenu.Core.Application.Families.Interfaces;
using SilentMike.DietMenu.Core.Application.Families.Models;
using SilentMike.DietMenu.Core.Domain.Entities;

public sealed record GetFamilyDataToImport : IRequest<FamilyDataToImport>, IGetFamilyDataToImport
{
    public CoreEntity Core { get; init; } = new(Guid.NewGuid());
    public IEnumerable<CoreIngredientTypeEntity> CoreIngredientTypes { get; init; } = new List<CoreIngredientTypeEntity>();
    public IEnumerable<CoreIngredientEntity> CoreIngredients { get; init; } = new List<CoreIngredientEntity>();
    public IEnumerable<CoreMealTypeEntity> CoreMealTypes { get; init; } = new List<CoreMealTypeEntity>();
    public FamilyEntity Family { get; init; } = new(Guid.NewGuid());
}
