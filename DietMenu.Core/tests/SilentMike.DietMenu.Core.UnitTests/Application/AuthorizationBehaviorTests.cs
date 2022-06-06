namespace SilentMike.DietMenu.Core.UnitTests.Application;

using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Behaviors;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

[TestClass]
public sealed class AuthorizationBehaviorTests
{
    [TestMethod]
    public async Task ShouldThrowUnauthorizedExceptionWhenMissingFamilyIdAndUserId()
    {
        //GIVEN
        var request = new UpsertIngredient();

        var service = new Mock<IAuthService>();
        service.Setup(i => i.CurrentUser)
            .Returns(() => (Guid.Empty, Guid.Empty));

        var behaviour = new AuthorizationBehavior<UpsertIngredient, Unit>(service.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .ThrowAsync<DietMenuUnauthorizedException>()
                .Where(i => i.Code == ErrorCodes.USER_NOT_FOUND)
            ;
    }


    [TestMethod]
    public async Task ShouldFillFamilyIdAndUserId()
    {
        //GIVEN
        var request = new UpsertIngredient();

        var service = new Mock<IAuthService>();
        service.Setup(i => i.CurrentUser)
            .Returns(() => (Guid.NewGuid(), Guid.NewGuid()));

        var behaviour = new AuthorizationBehavior<UpsertIngredient, Unit>(service.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }
}
