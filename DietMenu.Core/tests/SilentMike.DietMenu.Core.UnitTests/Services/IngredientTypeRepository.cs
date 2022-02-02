namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientTypeRepository : IIngredientTypeRepository
{
    private readonly Dictionary<Guid, IngredientTypeEntity> entities;

    public IngredientTypeRepository(Dictionary<Guid, IngredientTypeEntity>? entities = default)
    {
        entities ??= new Dictionary<Guid, IngredientTypeEntity>();
        this.entities = entities;
    }

    public Task<IngredientTypeEntity?> Get(Guid ingredientTypeId, CancellationToken cancellationToken = default)
    {
        var result = this.entities.ContainsKey(ingredientTypeId)
            ? this.entities[ingredientTypeId]
            : null;

        return Task.FromResult(result);
    }

    public Task<IEnumerable<IngredientTypeEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.entities.Values.Where(i => i.FamilyId == familyId);

        return Task.FromResult(result);
    }

    public Task Save(IngredientTypeEntity ingredientType, CancellationToken cancellationToken = default)
    {
        if (entities.ContainsKey(ingredientType.Id))
        {
            entities[ingredientType.Id] = ingredientType;
        }
        else
        {
            entities.Add(ingredientType.Id, ingredientType);
        }

        return Task.CompletedTask;
    }

    public Task Save(IEnumerable<IngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken = default)
    {
        foreach (var type in ingredientTypes)
        {
            this.Save(type, cancellationToken);
        }

        return Task.CompletedTask;
    }
}
