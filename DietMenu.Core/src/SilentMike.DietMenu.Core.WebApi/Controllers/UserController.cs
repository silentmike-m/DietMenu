namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Auth.Queries;
using SilentMike.DietMenu.Core.Application.Users.Commands;

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

    [AllowAnonymous]
    [HttpPost(Name = "Login")]
    public async Task<string> Login(GetToken login)
    {
        return await this.mediator.Send(login, CancellationToken.None);
    }
}
