namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
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

    public Task<IEnumerable<MealTypeEntity>> Get(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.entities.Values.Where(i => i.FamilyId == familyId);

        return Task.FromResult(result);
    }

    public Task Save(MealTypeEntity mealType, CancellationToken cancellationToken = default)
    {
        if (entities.ContainsKey(mealType.Id))
        {
            entities[mealType.Id] = mealType;
        }
        else
        {
            entities.Add(mealType.Id, mealType);
        }

        return Task.CompletedTask;
    }

    public Task Save(IEnumerable<MealTypeEntity> mealTypes, CancellationToken cancellationToken = default)
    {
        foreach (var mealType in mealTypes)
        {
            this.Save(mealType, cancellationToken);
        }

        return Task.CompletedTask;
    }
}
