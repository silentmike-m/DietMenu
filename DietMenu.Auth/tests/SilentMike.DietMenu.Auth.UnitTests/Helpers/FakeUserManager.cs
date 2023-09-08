namespace SilentMike.DietMenu.Auth.UnitTests.Helpers;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;

internal class FakeUserManager : UserManager<User>
{
    public FakeUserManager()
        : base(Substitute.For<IUserStore<User>>(),
            Substitute.For<IOptions<IdentityOptions>>(),
            Substitute.For<IPasswordHasher<User>>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            Substitute.For<ILookupNormalizer>(),
            Substitute.For<IdentityErrorDescriber>(),
            Substitute.For<IServiceProvider>(),
            new NullLogger<UserManager<User>>())
    {
    }
}
