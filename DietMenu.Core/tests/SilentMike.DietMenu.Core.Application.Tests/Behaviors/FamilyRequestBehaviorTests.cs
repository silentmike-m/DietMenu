namespace SilentMike.DietMenu.Core.Application.Tests.Behaviors;

using SilentMike.DietMenu.Core.Application.Common.Behaviors;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Common.Models;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Domain.Services;

[TestClass]
public sealed class FamilyRequestBehaviorTests
{
    private readonly IFamilyRepository familyRepository = Substitute.For<IFamilyRepository>();

    [TestMethod]
    public async Task Should_Not_Throw_Exception_When_Family_Exists()
    {
        //GIVEN
        var familyId = Guid.NewGuid();

        this.familyRepository
            .ExistsAsync(familyId, Arg.Any<CancellationToken>())
            .Returns(true);

        var request = new CreateIngredient
        {
            AuthData = new AuthData
            {
                FamilyId = familyId,
            },
        };

        var behavior = new FamilyRequestBehavior<CreateIngredient, Unit>(this.familyRepository);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    [TestMethod]
    public async Task Should_Throw_Family_Not_Found_Exception_When_Family_Not_Exists()
    {
        var familyId = Guid.NewGuid();

        this.familyRepository
            .ExistsAsync(familyId, Arg.Any<CancellationToken>())
            .Returns(false);

        var request = new CreateIngredient
        {
            AuthData = new AuthData
            {
                FamilyId = familyId,
            },
        };

        var behavior = new FamilyRequestBehavior<CreateIngredient, Unit>(this.familyRepository);

        //WHEN
        var action = async () => await behavior.Handle(request, () => Task.FromResult(Unit.Value), CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .WithMessage($"*{familyId}*")
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == familyId)
            ;
    }
}
