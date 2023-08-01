namespace SilentMike.DietMenu.Auth.UnitTests.Auth.QueryHandlers;

using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Auth.Queries;
using SilentMike.DietMenu.Auth.Application.Auth.ViewModels;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Infrastructure.Auth.QueryHandlers;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.UnitTests.Helpers;

[TestClass]
public sealed class GetUserClaimsHandlerTests
{
    private static readonly User USER = new()
    {
        Email = "user@domain.com",
        FamilyId = Guid.NewGuid(),
        Id = Guid.NewGuid().ToString(),
    };

    private readonly NullLogger<GetUserClaimsHandler> logger = new();
    private readonly Mock<FakeUserManager> userManager;

    public GetUserClaimsHandlerTests()
    {
        this.userManager = new FakeUserManagerBuilder()
            .With(manager => manager
                .Setup(service => service.FindByEmailAsync(USER.Email))
                .ReturnsAsync(USER)
            )
            .Build();
    }

    [TestMethod]
    public async Task Should_Return_User_Claims()
    {
        //GIVEN
        var request = new GetUserClaims
        {
            Email = USER.Email,
        };

        var handler = new GetUserClaimsHandler(this.logger, this.userManager.Object);

        //WHEN
        var result = await handler.Handle(request, CancellationToken.None);

        //THEN
        var expectedResult = new UserClaims
        {
            Claims = new Dictionary<string, string>
            {
                { JwtRegisteredClaimNames.Email, USER.Email },
                { JwtRegisteredClaimNames.Sub, USER.Id },
                { DietMenuClaimNames.FAMILY_ID, USER.FamilyId.ToString() },
                { DietMenuClaimNames.ROLE, USER.Role.ToString() },
                { DietMenuClaimNames.USER_ID, USER.Id },
            },
            FamilyId = USER.FamilyId,
            UserId = new Guid(USER.Id),
        };

        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedResult)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_User_Not_Found_When_Missing_User_With_Email()
    {
        //GIVEN
        var request = new GetUserClaims
        {
            Email = "fake@domai.com",
        };

        var handler = new GetUserClaimsHandler(this.logger, this.userManager.Object);

        //WHEN
        var action = async () => await handler.Handle(request, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<UserNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }
}
