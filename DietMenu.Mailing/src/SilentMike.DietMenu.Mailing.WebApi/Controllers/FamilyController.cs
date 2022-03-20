namespace SilentMike.DietMenu.Mailing.WebApi.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Mailing.Application.Family.Commands;

[ApiController]
[Route("[controller]/[action]")]
public sealed class FamilyController : ControllerBase
{
    private readonly IMediator mediator;

    public FamilyController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "SendImportedFamilyDataEmail")]
    public async Task<ActionResult> SendImportedFamilyDataEmail(SendImportedFamilyDataEmail request)
    {
        _ = await this.mediator.Send(request);

        return await Task.FromResult(Ok());
    }
}
