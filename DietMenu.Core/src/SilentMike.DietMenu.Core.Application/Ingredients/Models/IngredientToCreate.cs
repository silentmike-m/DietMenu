namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record IngredientToCreate
(
    decimal Exchanger,
    string Name,
    string Type,
    string? UnitSymbol = null
);
