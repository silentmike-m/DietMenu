namespace SilentMike.DietMenu.Auth.Web.Models;

using SilentMike.DietMenu.Auth.Web.Common.Constants;

public sealed record ResetPasswordConfirmationPageValues
{
    public string Area { get; init; } = IdentityPageNames.AREA;

    public Uri ReturnUrl { get; init; } = new("about:blank");
}
