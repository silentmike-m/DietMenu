namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous]
public sealed class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
        => this.logger = logger;

    [Route(""), HttpGet]
    [ApiExplorerSettings(IgnoreApi = true)]
    public RedirectResult Index()
    {
        this.logger.LogInformation("Redirect to Swagger");
        return Redirect("swagger/index.html");
    }
}
