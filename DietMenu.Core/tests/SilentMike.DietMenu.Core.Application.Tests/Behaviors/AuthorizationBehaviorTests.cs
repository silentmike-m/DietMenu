namespace SilentMike.DietMenu.Core.Application.Tests.Behaviors;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Behaviors;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

[TestClass]
public sealed class AuthorizationBehaviorTests
{
    private const string INVALID_ID = "00000000-0000-0000-0000-000000000000";
    private const string VALID_ID = "ceb8dd06-79e7-4bc1-81db-52aecbc6c132";

    private readonly IAuthService currentUserService = Substitute.For<IAuthService>();

    [TestMethod]
    public async Task Should_Not_Throw_Exception_When_Auth_Data_Are_Valid()
    {
        //GIVEN
        var familyId = new Guid(VALID_ID);
        var userId = new Guid(VALID_ID);

        this.currentUserService
            .CurrentUser
            .Returns((familyId, userId));

        var request = new CreateIngredient();

        var behavior = new AuthorizationBehavior<CreateIngredient, Unit>(this.currentUserService);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;

        var expectedAuthData = new AuthData
        {
            FamilyId = familyId,
            UserId = userId,
        };

        request.AuthData.Should()
            .BeEquivalentTo(expectedAuthData)
            ;

        _ = this.currentUserService.Received(1).CurrentUser;
    }

    [TestMethod]
    public async Task Should_Not_Validate_When_Auth_Data_Are_Not_Empty()
    {
        //GIVEN
        var request = new CreateIngredient
        {
            AuthData = new AuthData
            {
                FamilyId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            },
        };

        var behavior = new AuthorizationBehavior<CreateIngredient, Unit>(this.currentUserService);

        //WHEN
        await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        _ = this.currentUserService.Received(0).CurrentUser;
    }

    [DataRow(VALID_ID, INVALID_ID), DataRow(INVALID_ID, VALID_ID), DataTestMethod]
    public async Task Should_Throw_Diet_Menu_Unauthorized_Exception_When_Family_Id_Or_User_Id_Is_Empty(string familyIdString, string userIdString)
    {
        //GIVEN
        var familyId = new Guid(familyIdString);
        var userId = new Guid(userIdString);

        this.currentUserService
            .CurrentUser
            .Returns((familyId, userId));

        var request = new CreateIngredient();

        var behavior = new AuthorizationBehavior<CreateIngredient, Unit>(this.currentUserService);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<DietMenuUnauthorizedException>()
                .WithMessage($"*{familyIdString}*{userIdString}*")
                .Where(exception => exception.Code == ErrorCodes.UNAUTHORIZED)
            ;

        _ = this.currentUserService.Received(1).CurrentUser;
    }

    [DataRow(VALID_ID, INVALID_ID), DataRow(INVALID_ID, VALID_ID), DataTestMethod]
    public async Task Should_Validate_When_Family_Id_Or_User_Id_Is_Empty(string familyIdString, string userIdString)
    {
        //GIVEN
        var familyId = new Guid(familyIdString);
        var userId = new Guid(userIdString);

        this.currentUserService
            .CurrentUser
            .Returns((new Guid(VALID_ID), new Guid(VALID_ID)));

        var request = new CreateIngredient
        {
            AuthData = new AuthData
            {
                FamilyId = familyId,
                UserId = userId,
            },
        };

        var behavior = new AuthorizationBehavior<CreateIngredient, Unit>(this.currentUserService);

        //WHEN
        await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        _ = this.currentUserService.Received(1).CurrentUser;
    }
}
