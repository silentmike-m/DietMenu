namespace SilentMike.DietMenu.Core.UnitTests.Auth;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Auth.Queries;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Auth;
using SilentMike.DietMenu.Core.Infrastructure.Identity;
using SilentMike.DietMenu.Core.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Core.Infrastructure.Identity.QueryHandlers;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class GetTokenHandlerTests
{
    private const string UNCONFIRMED_EMAIL = "unconfirmed@domain.com";
    private const string EMAIL = "user@domain.com;";
    private const string PASSWORD = "Password";

    private readonly NullLogger<GetTokenHandler> logger;
    private readonly IOptions<JwtOptions> options;
    private readonly Mock<FakeUserManager> userManager;

    public GetTokenHandlerTests()
    {
        this.logger = new NullLogger<GetTokenHandler>();

        this.options = Options.Create<JwtOptions>(new JwtOptions
        {
            Audience = "audience",
            Issuer = "Issuer",
            SecurityKey = "v&Z4*bdJ=A$k^7BRE=247bCJFEWD5U_4",
        });

        var user = new DietMenuUser
        {
            Email = EMAIL,
            EmailConfirmed = true,
        };

        var unconfirmedUser = new DietMenuUser
        {
            Email = UNCONFIRMED_EMAIL,
            EmailConfirmed = false,
        };

        this.userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.Is<string>(s => s == EMAIL)))
                .Returns(Task.FromResult(user)))
            .With(i => i.Setup(m => m.FindByEmailAsync(It.Is<string>(s => s == UNCONFIRMED_EMAIL)))
                .Returns(Task.FromResult(unconfirmedUser)))
            .With(i => i.Setup(m => m.FindByEmailAsync(It.Is<string>(s => s != EMAIL && s != UNCONFIRMED_EMAIL)))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .With(i => i.Setup(m => m.CheckPasswordAsync(It.IsAny<DietMenuUser>(), PASSWORD))
                .Returns(Task.FromResult(true)))
            .Build();
    }

    [TestMethod]
    public async Task ShouldThrowTokenExceptionWhenInvalidEmail()
    {
        //GIVEN
        var request = new GetToken
        {
            Email = "email",
        };

        var requestHandler = new GetTokenHandler(this.options, this.logger, this.userManager.Object);

        //WHEN
        Func<Task<string>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<GetTokenException>()
                .Where(i => i.Code == ErrorCodes.GET_TOKEN)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowUnconfirmedEmailExceptionWhenEmailIsNotConfirmed()
    {
        //GIVEN
        var request = new GetToken
        {
            Email = UNCONFIRMED_EMAIL,
        };

        var requestHandler = new GetTokenHandler(this.options, this.logger, this.userManager.Object);

        //WHEN
        Func<Task<string>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UnconfirmedEmailException>()
                .Where(i => i.Code == ErrorCodes.UNCONFIRMED_EMAIL)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowTokenExceptionWhenInvalidPassword()
    {
        //GIVEN
        var request = new GetToken
        {
            Email = EMAIL,
            Password = "password",
        };

        var requestHandler = new GetTokenHandler(this.options, this.logger, this.userManager.Object);

        //WHEN
        Func<Task<string>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<GetTokenException>()
                .Where(i => i.Code == ErrorCodes.GET_TOKEN)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnToken()
    {
        //GIVEN
        var request = new GetToken
        {
            Email = EMAIL,
            Password = PASSWORD,
        };

        var requestHandler = new GetTokenHandler(this.options, this.logger, this.userManager.Object);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        result.Should()
            .NotBeNullOrWhiteSpace()
            ;
    }
}
