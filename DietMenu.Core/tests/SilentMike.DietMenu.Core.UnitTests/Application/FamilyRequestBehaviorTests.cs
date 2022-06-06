namespace SilentMike.DietMenu.Core.UnitTests.Application;

using SilentMike.DietMenu.Core.Application.Common.Behaviors;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class FamilyRequestBehaviorTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;

    public FamilyRequestBehaviorTests()
    {
        var family = new FamilyEntity(this.familyId);

        this.factory = new DietMenuDbContextFactory(family);

        this.familyRepository = new FamilyRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundWhenInvalidFamilyIdOnFamilyRequestBehavior()
    {
        //GIVEN
        var request = new DeleteIngredient
        {
            FamilyId = Guid.NewGuid(),
        };

        var behaviour = new FamilyRequestBehavior<DeleteIngredient, Unit>(this.familyRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(exception => exception.Code == ErrorCodes.FAMILY_NOT_FOUND)
                .Where(exception => exception.Id == request.FamilyId)
            ;
    }


    [TestMethod]
    public async Task ShouldNotThrowExceptionWhenFamilyExistsOnFamilyRequestBehavior()
    {
        //GIVEN
        var request = new DeleteIngredient
        {
            FamilyId = this.familyId,
        };

        var behaviour = new FamilyRequestBehavior<DeleteIngredient, Unit>(this.familyRepository);

        //WHEN
        Func<Task<Unit>> action = async () => await behaviour.Handle(request, CancellationToken.None, () => Task.FromResult(Unit.Value));

        //THEN
        await action.Should()
                .NotThrowAsync()
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
