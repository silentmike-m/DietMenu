namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal class FakeSignInManager : SignInManager<User>
{
    public FakeSignInManager(FakeUserManager userManager)
        : base(
            userManager,
            Substitute.For<IHttpContextAccessor>(),
            Substitute.For<IUserClaimsPrincipalFactory<User>>(),
            Substitute.For<IOptions<IdentityOptions>>(),
            new NullLogger<SignInManager<User>>(),
            Substitute.For<IAuthenticationSchemeProvider>(),
            Substitute.For<IUserConfirmation<User>>())
    {
    }
}
