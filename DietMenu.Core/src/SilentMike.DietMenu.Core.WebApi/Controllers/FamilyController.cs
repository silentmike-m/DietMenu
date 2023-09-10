namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Families.Queries;
using SilentMike.DietMenu.Core.WebApi.ViewModels.Family;

[ApiController, Authorize, Route("[controller]/[action]"), ExcludeFromCodeCoverage]
public sealed class FamilyController : ControllerBase
{
    private readonly ISender mediator;

    public FamilyController(ISender mediator)
        => this.mediator = mediator;

    [HttpGet(Name = "ingredientTypes")]
    public async Task<IReadOnlyList<IngredientType>> GetIngredientTypes(CancellationToken cancellationToken = default)
    {
        var ingredientTypes = await this.mediator.Send(new GetIngredientTypes(), cancellationToken);

        var result = ingredientTypes
            .Select(type => new IngredientType(type.Name, type.Type))
            .ToList();

        return result;
    }

    [HttpGet(Name = "mealTypes")]
    public async Task<IReadOnlyList<MealType>> GetMealTypes(CancellationToken cancellationToken = default)
    {
        var mealTypes = await this.mediator.Send(new GetMealTypes(), cancellationToken);

        var result = mealTypes
            .Select(type => new MealType(type.Name, type.Type))
            .ToList();

        return result;
    }
}
