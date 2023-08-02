namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal class FakeSignInManager : SignInManager<User>
{
    public FakeSignInManager(FakeUserManager userManager)
        : base(
            userManager,
            new Mock<IHttpContextAccessor>().Object,
            new Mock<IUserClaimsPrincipalFactory<User>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new NullLogger<SignInManager<User>>(),
            new Mock<IAuthenticationSchemeProvider>().Object,
            new Mock<IUserConfirmation<User>>().Object)
    {
    }
}
