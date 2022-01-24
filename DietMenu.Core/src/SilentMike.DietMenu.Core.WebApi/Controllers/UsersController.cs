namespace SilentMike.DietMenu.Core.WebApi.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Core.Application.Auth.Commands;
using SilentMike.DietMenu.Core.Application.Auth.Queries;

[ApiController]
[Route("[controller]/[action]")]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator mediator;

    public UsersController(IMediator mediator)
        => this.mediator = mediator;

    [AllowAnonymous]
    [HttpPost(Name = "ConfirmEmail")]
    public async Task<ActionResult> ConfirmEmail(ConfirmEmail request)
    {
        _ = await this.mediator.Send(request, CancellationToken.None);

        return await Task.FromResult(Ok());
    }

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
