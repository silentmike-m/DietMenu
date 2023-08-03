namespace SilentMike.DietMenu.Auth.UnitTests.IdentityServer.Services;

using System.Security.Claims;
using FluentAssertions;
using IdentityServer4.Models;
using MediatR;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Auth.Events;
using SilentMike.DietMenu.Auth.Application.Auth.Queries;
using SilentMike.DietMenu.Auth.Application.Auth.ViewModels;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.IdentityServer.Services;

[TestClass]
public sealed class ProfileServiceTests
{
    private const string FAMILY_ID = "b5405157-add0-4c14-96d0-43c8c381da15";
    private const string USER_EMAIL = "user@domain.com";
    private const string USER_ID = "1a67817c-86cf-46d4-b0be-87ac77e366ab";
    private const string USER_ROLE = "user_role";

    private readonly NullLogger<ProfileService> logger = new();
    private readonly Mock<IMediator> mediator = new();

    [TestMethod]
    public async Task Should_Add_User_Claim_On_Get_Profile_Data()
    {
        //GIVEN
        UserLoggedIn? userLoggedInNotification = null;

        var userClaims = new UserClaims
        {
            Claims = new Dictionary<string, string>
            {
                { DietMenuClaimNames.FAMILY_ID, FAMILY_ID },
                { DietMenuClaimNames.ROLE, USER_ROLE },
                { DietMenuClaimNames.USER_ID, USER_ID },
            },
            FamilyId = new Guid(FAMILY_ID),
            UserId = new Guid(USER_ID),
        };

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetUserClaims>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetUserClaims request, CancellationToken _) => request.Email == USER_EMAIL
                ? userClaims
                : new UserClaims()
            );

        this.mediator
            .Setup(service => service.Publish(It.IsAny<UserLoggedIn>(), It.IsAny<CancellationToken>()))
            .Callback<UserLoggedIn, CancellationToken>((notification, _) => userLoggedInNotification = notification);

        var emailClaim = new Claim(JwtRegisteredClaimNames.Email, USER_EMAIL);

        var claimsIdentity = new ClaimsIdentity(new List<Claim> { emailClaim });

        var context = new ProfileDataRequestContext
        {
            Subject = new ClaimsPrincipal(claimsIdentity),
        };

        var profileService = new ProfileService(this.logger, this.mediator.Object);

        //WHEN
        await profileService.GetProfileDataAsync(context);

        //THEN
        foreach (var userClaim in userClaims.Claims)
        {
            context.IssuedClaims.Should()
                .Contain(claim => claim.Type == userClaim.Key && claim.Value == userClaim.Value);
        }

        this.mediator.Verify(service => service.Publish(It.IsAny<UserLoggedIn>(), It.IsAny<CancellationToken>()), Times.Once);

        var expectedNotification = new UserLoggedIn
        {
            UserId = userClaims.UserId,
        };

        userLoggedInNotification.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expectedNotification)
            ;
    }

    [TestMethod]
    public async Task Should_Set_Is_Active_To_False_When_User_Email_Is_Not_Confirmed_On_Is_Active()
    {
        //GIVEN
        var userStatus = new UserStatus
        {
            IsEmailConfirmed = false,
            IsLockedOut = false,
        };

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetUserStatus>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userStatus);

        var emailClaim = new Claim(JwtRegisteredClaimNames.Email, USER_EMAIL);

        var claimsIdentity = new ClaimsIdentity(new List<Claim> { emailClaim });

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var context = new IsActiveContext(claimsPrincipal, new Mock<Client>().Object, "test");

        var profileService = new ProfileService(this.logger, this.mediator.Object);

        //WHEN
        await profileService.IsActiveAsync(context);

        //THEN
        context.IsActive.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Set_Is_Active_To_False_When_User_Is_Locked_Out_On_Is_Active()
    {
        //GIVEN
        var userStatus = new UserStatus
        {
            IsEmailConfirmed = true,
            IsLockedOut = true,
        };

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetUserStatus>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userStatus);

        var emailClaim = new Claim(JwtRegisteredClaimNames.Email, USER_EMAIL);

        var claimsIdentity = new ClaimsIdentity(new List<Claim> { emailClaim });

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var context = new IsActiveContext(claimsPrincipal, new Mock<Client>().Object, "test");

        var profileService = new ProfileService(this.logger, this.mediator.Object);

        //WHEN
        await profileService.IsActiveAsync(context);

        //THEN
        context.IsActive.Should()
            .BeFalse()
            ;
    }

    [TestMethod]
    public async Task Should_Set_Is_Active_To_True_When_User_Email_Is_Confirmed_And_User_Is_Not_Locked_Out_On_Is_Active()
    {
        //GIVEN
        var userStatus = new UserStatus
        {
            IsEmailConfirmed = true,
            IsLockedOut = false,
        };

        this.mediator
            .Setup(service => service.Send(It.IsAny<GetUserStatus>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userStatus);

        var emailClaim = new Claim(JwtRegisteredClaimNames.Email, USER_EMAIL);

        var claimsIdentity = new ClaimsIdentity(new List<Claim> { emailClaim });

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var context = new IsActiveContext(claimsPrincipal, new Mock<Client>().Object, "test");

        var profileService = new ProfileService(this.logger, this.mediator.Object);

        //WHEN
        await profileService.IsActiveAsync(context);

        //THEN
        context.IsActive.Should()
            .BeTrue()
            ;
    }
}
