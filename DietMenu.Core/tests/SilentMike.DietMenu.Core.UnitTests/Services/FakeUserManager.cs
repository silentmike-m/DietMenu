namespace SilentMike.DietMenu.Core.UnitTests.Services;

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;

internal class FakeUserManager : UserManager<DietMenuUser>
{
    public FakeUserManager()
        : base(new Mock<IUserStore<DietMenuUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<DietMenuUser>>().Object,
            Array.Empty<IUserValidator<DietMenuUser>>(),
            Array.Empty<IPasswordValidator<DietMenuUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<DietMenuUser>>>().Object)
    {

    }
}
