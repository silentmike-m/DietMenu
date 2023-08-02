namespace SilentMike.DietMenu.Auth.UnitTests.Behaviors;

using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Behaviors;
using SilentMike.DietMenu.Auth.Application.Common.Models;
using SilentMike.DietMenu.Auth.Application.Exceptions;
using SilentMike.DietMenu.Auth.Application.Families.Commands;

[TestClass]
public sealed class AuthorizationBehaviorTests
{
    private readonly Mock<ICurrentRequestService> currentRequestService = new();

    [TestMethod]
    public async Task Should_Not_Call_Current_Request_Service_When_FamilyId_And_UserId_Are_Not_Empty()
    {
        //GIVEN
        var request = new CreateFamily
        {
            AuthData = new AuthData
            {
                FamilyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            },
        };

        var behavior = new AuthorizationBehavior<CreateFamily, Unit>(this.currentRequestService.Object);

        //WHEN
        await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        this.currentRequestService.Verify(service => service.CurrentUser, Times.Never);
    }

    [TestMethod]
    public async Task Should_Pass_Validation_When_FamilyId_And_UserId_Are_Not_Empty()
    {
        //GIVEN
        var familyId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        this.currentRequestService
            .Setup(service => service.CurrentUser)
            .Returns((familyId, userId));

        var request = new CreateFamily();

        var behavior = new AuthorizationBehavior<CreateFamily, Unit>(this.currentRequestService.Object);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;

        request.AuthData.FamilyId.Should()
            .Be(familyId)
            ;

        request.AuthData.UserId.Should()
            .Be(userId)
            ;
    }

    [TestMethod]
    public async Task Should_Throw_DietMenu_Unauthorized_Exception_When_FamilyId_Is_Empty()
    {
        //GIVEN
        var familyId = Guid.Empty;
        var userId = Guid.NewGuid();

        this.currentRequestService
            .Setup(service => service.CurrentUser)
            .Returns((familyId, userId));

        var request = new CreateFamily();

        var behavior = new AuthorizationBehavior<CreateFamily, Unit>(this.currentRequestService.Object);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<DietMenuUnauthorizedException>()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_DietMenu_Unauthorized_Exception_When_UserId_Is_Empty()
    {
        //GIVEN
        var familyId = Guid.NewGuid();
        var userId = Guid.Empty;

        this.currentRequestService
            .Setup(service => service.CurrentUser)
            .Returns((familyId, userId));

        var request = new CreateFamily();

        var behavior = new AuthorizationBehavior<CreateFamily, Unit>(this.currentRequestService.Object);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<DietMenuUnauthorizedException>()
            ;
    }
}
