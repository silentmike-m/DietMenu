namespace SilentMike.DietMenu.Auth.Web.Services;

using SilentMike.DietMenu.Auth.Infrastructure.Identity.Interfaces;
using SilentMike.DietMenu.Auth.Web.Common.Constants;
using SilentMike.DietMenu.Auth.Web.Models;

internal sealed class IdentityPageUrlService : IIdentityPageUrlService
{
    private readonly LinkGenerator linkGenerator;

    public IdentityPageUrlService(LinkGenerator linkGenerator)
        => this.linkGenerator = linkGenerator;

    public string GetConfirmUserEmailPageUrl(Uri hostUri, Uri returnHostUri, string token, Guid userId)
    {
        var values = new ConfirmUserEmailPageValues
        {
            ReturnUrl = returnHostUri,
            Token = token,
            UserId = userId,
        };

        var url = this.GetUrl(hostUri, IdentityPageNames.COMPLETE_USER_REGISTRATION, values);

        return url;
    }

    private string GetUrl(Uri host, string pageName, object values)
    {
        var hostString = host.IsDefaultPort
            ? new HostString(host.Host)
            : new HostString(host.Host, host.Port);

        var scheme = host.Scheme;

        var url = this.linkGenerator.GetUriByPage(
            pageName,
            handler: null,
            values,
            scheme,
            hostString);

        url ??= string.Empty;

        return url;
    }
}
