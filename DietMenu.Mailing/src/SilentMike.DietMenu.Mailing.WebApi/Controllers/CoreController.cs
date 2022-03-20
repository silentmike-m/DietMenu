namespace SilentMike.DietMenu.Mailing.WebApi.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Mailing.Application.Core.Commands;

[ApiController]
[Route("[controller]/[action]")]
public sealed class CoreController : ControllerBase
{
    private readonly IMediator mediator;

    public CoreController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "SendImportedCoreDataEmail")]
    public async Task<ActionResult> SendImportedCoreDataEmail(SendImportedCoreDataEmail request)
    {
        _ = await this.mediator.Send(request);

        return await Task.FromResult(Ok());
    }
}
