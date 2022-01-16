namespace DietMenu.Core.WebApi.Controllers;

using DietMenu.Core.Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]/[action]")]
public sealed class UserController : ControllerBase
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
        => this.mediator = mediator;

    [AllowAnonymous]
    [HttpPost(Name = "CreateUser")]
    public async Task<ActionResult> CreateUser(CreateUser request)
    {
        _ = await this.mediator.Send(request);

        return await Task.FromResult(Ok());
    }
}
