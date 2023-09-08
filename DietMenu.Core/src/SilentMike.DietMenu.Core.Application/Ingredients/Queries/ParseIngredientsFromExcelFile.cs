namespace SilentMike.DietMenu.Core.Application.Ingredients.Queries;

using SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record ParseIngredientsFromExcelFile(string IngredientType, byte[] Payload) : IRequest<IReadOnlyList<IngredientToImport>>;
