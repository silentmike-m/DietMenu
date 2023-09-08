namespace SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record IngredientToImport
(
    double Exchanger,
    Guid Id,
    string Name,
    string UnitSymbol
);
