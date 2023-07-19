namespace SilentMike.DietMenu.Mailing.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController, Route("[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? mediator;
    protected ISender Mediator => this.mediator ??= this.HttpContext.RequestServices.GetRequiredService<ISender>();
}
