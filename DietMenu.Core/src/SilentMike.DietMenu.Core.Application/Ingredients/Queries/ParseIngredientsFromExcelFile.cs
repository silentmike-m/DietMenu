namespace SilentMike.DietMenu.Core.Application.Ingredients.Queries;

using SilentMike.DietMenu.Core.Application.Ingredients.Models;

public sealed record ParseIngredientsFromExcelFile : IRequest<IReadOnlyList<IngredientFromExcelFile>>
{
    public string IngredientType { get; init; } = string.Empty;
    public byte[] Payload { get; init; } = Array.Empty<byte>();
}
