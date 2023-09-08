namespace SilentMike.DietMenu.Auth.UnitTests.Behaviors;

using SilentMike.DietMenu.Auth.Application.Common;
using SilentMike.DietMenu.Auth.Application.Common.Behaviors;
using SilentMike.DietMenu.Auth.Application.Exceptions;
using SilentMike.DietMenu.Auth.Application.Families.Commands;
using SilentMike.DietMenu.Auth.Domain.Enums;

[TestClass]
public sealed class SystemBehaviorTests
{
    private readonly ICurrentRequestService currentRequestService = Substitute.For<ICurrentRequestService>();

    [TestMethod]
    public async Task Should_Pass_When_User_Role_Is_System()
    {
        //GIVEN
        var userRole = UserRole.System.ToString();

        this.currentRequestService
            .CurrentUserRole
            .Returns(userRole);

        var request = new CreateFamily();

        var behavior = new SystemBehavior<CreateFamily, Unit>(this.currentRequestService);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Invalid_Role_Exception_When_User_Role_Is_Not_System()
    {
        //GIVEN
        var userRole = UserRole.User.ToString();

        this.currentRequestService
            .CurrentUserRole
            .Returns(userRole);

        var request = new CreateFamily();

        var behavior = new SystemBehavior<CreateFamily, Unit>(this.currentRequestService);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<InvalidRoleException>()
                .WithMessage($"*{userRole}*")
            ;
    }
}
