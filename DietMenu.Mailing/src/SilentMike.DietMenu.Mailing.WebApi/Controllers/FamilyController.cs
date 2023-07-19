namespace SilentMike.DietMenu.Mailing.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;

[ApiController, Route("[controller]/[action]")]
public sealed class FamilyController : ApiControllerBase
{
    [HttpPost(Name = "SendImportedFamilyDataEmail")]
    public async Task<ActionResult> SendImportedFamilyDataEmail(SendImportedFamilyDataEmail request, CancellationToken cancellationToken = default)
    {
        await this.Mediator.Send(request, cancellationToken);

        return await Task.FromResult(this.Ok());
    }
}
