namespace SilentMike.DietMenu.Auth.UnitTests.Users;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common.Constants;
using SilentMike.DietMenu.Auth.Application.Exceptions.Users;
using SilentMike.DietMenu.Auth.Application.Users.Queries;
using SilentMike.DietMenu.Auth.Application.Users.ViewModels;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.Models;
using SilentMike.DietMenu.Auth.Infrastructure.Identity.QueryHandlers;
using SilentMike.DietMenu.Auth.UnitTests.Services;

[TestClass]
public sealed class GetInformationAboutMyselfHandlerTests
{
    private readonly NullLogger<GetInformationAboutMyselfHandler> logger = new();

    [TestMethod]
    public async Task ShouldThrowUserNotFoundWhenMissingUserOnGetInformationAboutMyself()
    {
        //GIVEN
        var userManager = new FakeUserManagerBuilder()
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                .Returns(Task.FromResult<DietMenuUser>(null)))
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            .Build();

        var request = new GetInformationAboutMyself();
        var requestHandler = new GetInformationAboutMyselfHandler(this.logger, userManager.Object);

        //WHEN
        Func<Task<User>> action = async () => await requestHandler.Handle(request, CancellationToken.None);

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
            .With(i => i.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(user)))
            .Build();

        var request = new GetInformationAboutMyself();
        var requestHandler = new GetInformationAboutMyselfHandler(this.logger, userManager.Object);

        //WHEN
        var result = await requestHandler.Handle(request, CancellationToken.None);

        //THEN
        result.Should()
            .BeEquivalentTo(user, opt => opt.ExcludingMissingMembers())
            ;
        result.IsActivated.Should()
            .Be(user.EmailConfirmed)
            ;
    }
}
