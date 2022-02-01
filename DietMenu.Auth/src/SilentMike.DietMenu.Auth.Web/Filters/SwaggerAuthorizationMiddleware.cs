namespace SilentMike.DietMenu.Auth.Web.Filters;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

[ExcludeFromCodeCoverage]
internal sealed class SwaggerAuthorizationMiddleware
{
    private readonly RequestDelegate next;

    public SwaggerAuthorizationMiddleware(RequestDelegate next) => this.next = next;

    public async Task Invoke(HttpContext httpContext)
    {
        var isSwagger = httpContext.Request.Path.Value?.Contains("/swagger", StringComparison.InvariantCultureIgnoreCase)
                        ?? false;

        if (isSwagger)
        {
            var result = await httpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                await httpContext.ChallengeAsync();
                return;
            }
        }

        await this.next.Invoke(httpContext);
    }
}
