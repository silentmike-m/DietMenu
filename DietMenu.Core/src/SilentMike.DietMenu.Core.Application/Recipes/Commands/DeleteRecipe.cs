namespace SilentMike.DietMenu.Core.Application.Recipes.Commands;

using SilentMike.DietMenu.Core.Application.Common;

public sealed record DeleteRecipe : IRequest, IAuthRequest, IFamilyRequest
{
    [JsonPropertyName("id")] public Guid Id { get; init; } = Guid.Empty;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
