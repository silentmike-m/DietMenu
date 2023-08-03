namespace SilentMike.DietMenu.Auth.Web.Models;

using SilentMike.DietMenu.Auth.Web.Common.Constants;

internal sealed record ForgotPasswordConfirmationPageValues
{
    public string Area { get; init; } = IdentityPageNames.AREA;
}
