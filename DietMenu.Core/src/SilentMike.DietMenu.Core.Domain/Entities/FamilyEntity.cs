﻿namespace SilentMike.DietMenu.Core.Domain.Entities;

public sealed class FamilyEntity
{
    public FamilyEntity() { }

    public FamilyEntity(Guid id) => this.Id = id;

    public Guid Id { get; private set; }
    public List<MealTypeEntity> MealTypes { get; set; } = new();
    public string Name { get; set; } = string.Empty;
}
