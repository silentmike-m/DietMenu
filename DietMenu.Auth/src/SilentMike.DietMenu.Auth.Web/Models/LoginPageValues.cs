namespace SilentMike.DietMenu.Auth.Web.Models;

using SilentMike.DietMenu.Auth.Web.Common.Constants;

internal sealed record LoginPageValues
{
    public string Area { get; init; } = IdentityPageNames.AREA;
    public string? ReturnUrl { get; init; } = string.Empty;
}
