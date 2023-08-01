namespace SilentMike.DietMenu.Auth.Web.Controllers;

using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Auth.Application.Users.Commands;
using SilentMike.DietMenu.Auth.Web.Common.Constants;

[ApiController, Authorize, Authorize(IdentityServerConstants.LocalApi.PolicyName), Authorize(Policy = PolicyNames.SYSTEM), Route("[controller]/[action]")]
public sealed class UserController : ControllerBase
{
    private readonly ISender mediator;

    public UserController(ISender mediator)
        => this.mediator = mediator;

    [HttpPost(Name = "CreateUser")]
    public async Task<IActionResult> CreateFamily(CreateUser request, CancellationToken cancellationToken = default)
    {
        await this.mediator.Send(request, cancellationToken);

        return await Task.FromResult(this.Ok());
    }
}
