namespace SilentMike.DietMenu.Core.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.MealTypes.Commands;

[ApiController, Authorize, Route("[controller]/[action]")]
public sealed class MealTypesController : ControllerBase
{
    private readonly IMediator mediator;

    public MealTypesController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "UpsertMealTypes")]
    public async Task<ActionResult> UpsertMealTypes([FromBody] UpsertMealTypes request)
    {
        if (request.MealTypes.Any())
        {
            _ = await this.mediator.Send(request, CancellationToken.None);
        }

        return await Task.FromResult(Ok());
    }
}
