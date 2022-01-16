﻿namespace DietMenu.Core.UnitTests.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DietMenu.Core.Domain.Entities;
using DietMenu.Core.Domain.Repositories;

internal sealed class FamilyRepository : IFamilyRepository
{
    private readonly Dictionary<Guid, FamilyEntity> entities;

    public FamilyRepository(Dictionary<Guid, FamilyEntity>? entities = default)
    {
        entities ??= new Dictionary<Guid, FamilyEntity>();
        this.entities = entities;
    }

    public Task<FamilyEntity?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        var result = this.entities.ContainsKey(id)
            ? this.entities[id]
            : null;

        return Task.FromResult(result);
    }

    public Task<FamilyEntity?> Get(string name, CancellationToken cancellationToken = default)
    {
        var result = this.entities.Values.SingleOrDefault(i => i.Name == name);

        return Task.FromResult(result);
    }

    public Task Save(FamilyEntity family, CancellationToken cancellationToken = default)
    {
        if (entities.ContainsKey(family.Id))
        {
            entities[family.Id] = family;
        }
        else
        {
            entities.Add(family.Id, family);
        }

        return Task.CompletedTask;
    }
}