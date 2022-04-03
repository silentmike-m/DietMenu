namespace SilentMike.DietMenu.Core.UnitTests.Application;

using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Behaviours;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;

[TestClass]
public sealed class AuthorizationBehaviourTests
{
    [TestMethod]
    public async Task ShouldThrowUnauthorizedExceptionWhenMissingFamilyIdAndUserId()
    {
        //GIVEN
        var request = new UpsertIngredient();

        var service = new Mock<ICurrentRequestService>();
        service.Setup(i => i.CurrentUser)
            .Returns(() => (Guid.Empty, Guid.Empty));

        var behaviour = new AuthorizationBehaviour<UpsertIngredient, Unit>(service.Object);

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

        var service = new Mock<ICurrentRequestService>();
        service.Setup(i => i.CurrentUser)
            .Returns(() => (Guid.NewGuid(), Guid.NewGuid()));

        var behaviour = new AuthorizationBehaviour<UpsertIngredient, Unit>(service.Object);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }
}
