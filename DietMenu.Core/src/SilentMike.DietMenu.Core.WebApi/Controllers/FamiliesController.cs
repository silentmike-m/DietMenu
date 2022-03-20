namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Families.Commands;

[ApiController, AllowAnonymous, Route("[controller]/[action]")]
public sealed class FamiliesController : ControllerBase
{
    private readonly IMediator mediator;

    public FamiliesController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "CreateFamily")]
    public async Task<ActionResult> CreateFamily([FromBody] CreateFamily request)
    {
        _ = await this.mediator.Send(request, CancellationToken.None);

        return await Task.FromResult(Ok());
    }
}
