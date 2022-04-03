namespace SilentMike.DietMenu.Auth.Web.Controllers;

using IdentityServer4;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;

[ApiController, Authorize(IdentityServerConstants.LocalApi.PolicyName), Authorize("System"), Route("[controller]/[action]")]
public sealed class UserController : Controller
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator) => this.mediator = mediator;

    [HttpPost(Name = "GetInformationAboutMyself")]
    public async Task<User> GetInformationAboutMyself(GetInformationAboutMyself request)
     => await this.mediator.Send(request, CancellationToken.None);
}
