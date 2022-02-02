namespace SilentMike.DietMenu.Core.Application.Ingredients.Queries;

using SilentMike.DietMenu.Core.Domain.Entities;

public sealed record ParseIngredientsFromExcelFile : IRequest<IReadOnlyList<IngredientEntity>>
{
    public Guid FamilyId { get; init; } = Guid.Empty;
    public byte[] Payload { get; init; } = Array.Empty<byte>();
    public Guid TypeId { get; init; } = Guid.Empty;
    public string TypeInternalName { get; init; } = string.Empty;
}
