namespace SilentMike.DietMenu.Core.WebApi.Controllers;

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[AllowAnonymous, ExcludeFromCodeCoverage]
public sealed class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
        => this.logger = logger;

    [Route(""), HttpGet, ApiExplorerSettings(IgnoreApi = true)]
    public RedirectResult Index()
    {
        this.logger.LogInformation("Redirect to Swagger");

        return this.Redirect("swagger/index.html");
    }
}
