namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Queries;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels;

[ApiController, Authorize, Route("[controller]/[action]")]
public sealed class IngredientsController : ControllerBase
{
    private readonly IMediator mediator;

    public IngredientsController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "DeleteIngredient")]
    public async Task<ActionResult> DeleteIngredient([FromBody] DeleteIngredient request)
    {
        _ = await this.mediator.Send(request, CancellationToken.None);

        return await Task.FromResult(Ok());
    }

    [HttpPost(Name = "GetIngredientsGrid")]
    public async Task<IngredientsGrid> GetIngredientsGrid([FromBody] GetIngredientsGrid request)
        => await this.mediator.Send(request, CancellationToken.None);


    [HttpPost(Name = "UpsertIngredients")]
    public async Task<ActionResult> UpsertIngredients([FromBody] UpsertIngredient request)
    {
        _ = await this.mediator.Send(request, CancellationToken.None);

        return await Task.FromResult(Ok());
    }
}
