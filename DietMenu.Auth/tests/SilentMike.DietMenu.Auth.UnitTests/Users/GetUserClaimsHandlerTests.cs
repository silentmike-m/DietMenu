namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Models;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.QueryHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Services;

[TestClass]
public sealed class GetUserClaimsHandlerTests
{
    private readonly NullLogger<GetUserClaimsHandler> logger = new();

    [TestMethod]
    public async Task ShouldThrowUserNotFoundWhenMissingUserOnGetUser()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .Build();

        var request = new GetUserClaims();
        var requestHandler = new GetUserClaimsHandler(this.logger, userManager.Object);

        //WHEN
        Func<Task<UserClaims>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }

    [TestMethod]
    public async Task ShouldReturnUserOnGetInformationAboutMyself()
    {
        //GIVEN
        var user = new DietMenuUser
        {
            Id = Guid.NewGuid().ToString(),
            Email = "user@domain.com",
            EmailConfirmed = true,
            FamilyId = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Smith",
            PhoneNumber = "+48",
        };

        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .Build();

        var request = new GetUserClaims();
        var requestHandler = new GetUserClaimsHandler(this.logger, userManager.Object);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        result.Claims.Should()
            .HaveCount(4)
            .And
            .Contain(claim => claim.Type == JwtRegisteredClaimNames.Email && claim.Value == user.Email)
            .And
            .Contain(claim => claim.Type == JwtRegisteredClaimNames.Sub && claim.Value == user.Id)
            .And
            .Contain(claim => claim.Type == DietMenuClaimNames.FamilyId && claim.Value == user.FamilyId.ToString())
            .And
            .Contain(claim => claim.Type == DietMenuClaimNames.UserId && claim.Value == user.Id)
            ;
        result.UserId.Should()
            .Be(user.Id)
            ;
    }
}
