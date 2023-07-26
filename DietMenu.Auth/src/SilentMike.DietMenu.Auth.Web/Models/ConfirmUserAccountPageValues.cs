﻿namespace SilentMike.DietMenu.Auth.Web.Models;

using SilentMike.DietMenu.Auth.Web.Common.Constants;

internal sealed record ConfirmUserEmailPageValues
{
    public string Area { get; init; } = IdentityPageNames.AREA;
    public Uri ReturnUrl { get; init; } = new("about:blank");
    public string Token { get; init; } = string.Empty;
    public Guid UserId { get; init; } = Guid.Empty;
}
