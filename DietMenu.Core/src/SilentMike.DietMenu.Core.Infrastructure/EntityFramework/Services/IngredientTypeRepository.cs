﻿namespace SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;

using Microsoft.EntityFrameworkCore;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Data;

internal sealed class IngredientTypeRepository : IIngredientTypeRepository
{
    private readonly IDietMenuDbContext context;

    public IngredientTypeRepository(IDietMenuDbContext context) => (this.context) = (context);

    public async Task<IngredientTypeEntity?> Get(Guid familyId, Guid ingredientTypeId, CancellationToken cancellationToken = default)
    {
        return await this.context.IngredientTypes
            .SingleOrDefaultAsync(i => i.FamilyId == familyId && i.Id == ingredientTypeId, cancellationToken);
    }

    public async Task<IEnumerable<IngredientTypeEntity>> Get(Guid familyId, CancellationToken cancellationToken = default)
    {
        var result = this.context.IngredientTypes.Where(i => i.FamilyId == familyId);

        return await Task.FromResult(result);
    }

    public async Task Save(IngredientTypeEntity ingredientType, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredientType, cancellationToken);
    }

    public async Task Save(IEnumerable<IngredientTypeEntity> ingredientTypes, CancellationToken cancellationToken = default)
    {
        await this.context.Save(ingredientTypes, cancellationToken);
    }
}
