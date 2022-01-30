namespace SilentMike.DietMenu.Auth.Web.Services;

using Microsoft.AspNetCore.Http;
using SilentMike.DietMenu.Auth.Application.Common;

internal sealed class CurrentRequestService : ICurrentRequestService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public CurrentRequestService(IHttpContextAccessor httpContextAccessor) =>
        this.httpContextAccessor = httpContextAccessor;

    public string Schema => this.httpContextAccessor!.HttpContext!.Request.Scheme;
}
