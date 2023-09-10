﻿namespace SilentMike.DietMenu.Core.WebApi.ViewModels.Family;

public sealed record IngredientType
{
    [JsonPropertyName("name")] public string Name { get; init; } = string.Empty;
    [JsonPropertyName("type")] public string Type { get; init; } = string.Empty;

    public IngredientType(string name, string type)
    {
        this.Name = name;
        this.Type = type;
    }
}
