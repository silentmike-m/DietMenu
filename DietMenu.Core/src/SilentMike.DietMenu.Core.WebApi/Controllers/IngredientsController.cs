namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

[ApiController, Authorize, Route("[controller]/[action]")]
public sealed class IngredientsController : ControllerBase
{
    private readonly IMediator mediator;

    public IngredientsController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "UpsertIngredients")]
    public async Task<ActionResult> UpsertIngredients([FromBody] UpsertIngredients request)
    {
        if (request.Ingredients.Any())
        {
            _ = await this.mediator.Send(request, CancellationToken.None);
        }

        return await Task.FromResult(Ok());
    }
}
