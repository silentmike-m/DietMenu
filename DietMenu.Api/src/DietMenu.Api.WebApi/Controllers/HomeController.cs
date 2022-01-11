using Microsoft.AspNetCore.Mvc;

namespace DietMenu.Api.WebApi.Controllers;

using Microsoft.AspNetCore.Authorization;

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
