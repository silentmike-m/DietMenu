namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Commands;
using SilentMike.DietMenu.Core.Application.IngredientTypes.Queries;
using SilentMike.DietMenu.Core.Application.IngredientTypes.ViewModels;

[ApiController, Authorize, Route("[controller]/[action]")]
public sealed class IngredientTypesController : ControllerBase
{
    private readonly IMediator mediator;

    public IngredientTypesController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "GetIngredientTypesGrid")]
    public async Task<IngredientTypesGrid> GetIngredientTypesGrid([FromBody] GetIngredientTypesGrid request)
    {
        return await this.mediator.Send(request, CancellationToken.None);
    }

    [HttpPost(Name = "UpsertIngredientTypes")]
    public async Task<ActionResult> UpsertIngredientTypes([FromBody] UpsertIngredientType request)
    {
        _ = await this.mediator.Send(request, CancellationToken.None);

        return await Task.FromResult(Ok());
    }
}
