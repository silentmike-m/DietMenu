namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class MealTypeRepository : IMealTypeRepository
{
    private readonly Dictionary<Guid, MealTypeEntity> entities;

    public MealTypeRepository(Dictionary<Guid, MealTypeEntity>? entities = default)
    {
        entities ??= new Dictionary<Guid, MealTypeEntity>();
        this.entities = entities;
    }

    public Task<MealTypeEntity?> Get(Guid familyId, Guid mealTypeId, CancellationToken cancellationToken = default)
    {
        var mealType = this.entities.ContainsKey(mealTypeId)
            ? this.entities[mealTypeId]
            : null;

        var result = mealType?.FamilyId == familyId
            ? mealType
            : null;

        return Task.FromResult(result);
    }

    public Task Save(MealTypeEntity mealType, CancellationToken cancellationToken = default)
    {
        if (entities.ContainsKey(mealType.MealTypeId))
        {
            entities[mealType.MealTypeId] = mealType;
        }
        else
        {
            entities.Add(mealType.MealTypeId, mealType);
        }

        return Task.CompletedTask;
    }
}
