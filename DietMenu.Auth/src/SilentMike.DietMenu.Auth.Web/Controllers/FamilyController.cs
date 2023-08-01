namespace SilentMike.DietMenu.Auth.Web.Controllers;

using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Web.Common.Constants;

[ApiController, Authorize, Authorize(IdentityServerConstants.LocalApi.PolicyName), Authorize(Policy = PolicyNames.SYSTEM), Route("[controller]/[action]")]
public sealed class FamilyController : ControllerBase
{
    private readonly ISender mediator;

    public FamilyController(ISender mediator)
        => this.mediator = mediator;

    [HttpPost(Name = "CreateFamily")]
    public async Task<IActionResult> CreateFamily(CreateFamily request, CancellationToken cancellationToken = default)
    {
        await this.mediator.Send(request, cancellationToken);

        return await Task.FromResult(this.Ok());
    }
}
