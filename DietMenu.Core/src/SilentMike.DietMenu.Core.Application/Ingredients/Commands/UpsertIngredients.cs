namespace SilentMike.DietMenu.Core.Application.Ingredients.Commands;

using System.Text.Json.Serialization;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;

public sealed record UpsertIngredients : IRequest, IAuthRequest
{
    [JsonPropertyName("ingredients")]
    public IReadOnlyList<IngredientToUpsert> Ingredients { get; init; } = new List<IngredientToUpsert>().AsReadOnly();
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
