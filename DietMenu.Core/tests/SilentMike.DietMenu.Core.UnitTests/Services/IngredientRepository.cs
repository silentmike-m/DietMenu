namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class IngredientRepository : IIngredientRepository
{
    private readonly Dictionary<Guid, IngredientEntity> entities;

    public IngredientRepository(Dictionary<Guid, IngredientEntity>? entities = default)
    {
        entities ??= new Dictionary<Guid, IngredientEntity>();
        this.entities = entities;
    }

    public Task<IngredientEntity?> Get(Guid ingredientId, CancellationToken cancellationToken = default)
    {
        var result = this.entities.ContainsKey(ingredientId)
            ? this.entities[ingredientId]
            : null;

        return Task.FromResult(result);
    }

    public Task<IEnumerable<IngredientEntity>> GetByFamilyId(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.entities.Values.Where(i => i.FamilyId == familyId);

        return Task.FromResult(result);
    }

    public Task Save(IngredientEntity ingredient, CancellationToken cancellationToken = default)
    {
        if (entities.ContainsKey(ingredient.Id))
        {
            entities[ingredient.Id] = ingredient;
        }
        else
        {
            entities.Add(ingredient.Id, ingredient);
        }

        return Task.CompletedTask;
    }

    public Task Save(IEnumerable<IngredientEntity> ingredients, CancellationToken cancellationToken = default)
    {
        foreach (var type in ingredients)
        {
            this.Save(type, cancellationToken);
        }

        return Task.CompletedTask;
    }
}
