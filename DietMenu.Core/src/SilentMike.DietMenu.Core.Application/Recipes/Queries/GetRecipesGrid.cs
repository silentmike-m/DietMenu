namespace SilentMike.DietMenu.Core.Application.Recipes.Queries;

using System;
using System.Text.Json.Serialization;
using MediatR;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels;

public sealed record GetRecipesGrid : IRequest<RecipesGrid>, IAuthRequest
{
    [JsonPropertyName("grid_request")] public GridRequest GridRequest { get; init; } = new();
    [JsonPropertyName("ingredient_filter")] public string? IngredientFilter { get; init; } = default;
    [JsonPropertyName("meal_type_id")] public Guid? MealTypeId { get; init; } = default;
    [JsonIgnore] public Guid FamilyId { get; set; } = Guid.Empty;
    [JsonIgnore] public Guid UserId { get; set; } = Guid.Empty;
}
