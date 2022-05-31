namespace SilentMike.DietMenu.Core.UnitTests.Ingredients;

using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Services;
using SilentMike.DietMenu.Core.UnitTests.Services;

[TestClass]
public sealed class DeleteIngredientHandlerTests : IDisposable
{
    private readonly Guid familyId = Guid.NewGuid();
    private readonly Guid ingredientId = Guid.NewGuid();
    private readonly Guid systemIngredientId = Guid.NewGuid();

    private readonly DietMenuDbContextFactory factory;
    private readonly FamilyRepository familyRepository;
    private readonly IngredientRepository ingredientRepository;
    private readonly NullLogger<DeleteIngredientHandler> logger;


    public DeleteIngredientHandlerTests()
    {
        var family = new FamilyEntity(this.familyId);

        var ingredientType = new IngredientTypeEntity(Guid.NewGuid())
        {
            FamilyId = family.Id,
        };

        var ingredient = new IngredientEntity(this.ingredientId)
        {
            FamilyId = family.Id,
            InternalName = "ingredient",
            Name = "ingredient",
            TypeId = ingredientType.Id,
        };

        var systemIngredient = new IngredientEntity(this.systemIngredientId)
        {
            FamilyId = family.Id,
            InternalName = "system ingredient",
            Name = "system ingredient",
            TypeId = ingredientType.Id,
        };

        this.factory = new DietMenuDbContextFactory(family, ingredientType, ingredient, systemIngredient);

        this.familyRepository = new FamilyRepository(this.factory.Context);
        this.logger = new NullLogger<DeleteIngredientHandler>();
        this.ingredientRepository = new IngredientRepository(this.factory.Context);
    }

    [TestMethod]
    public async Task ShouldThrowFamilyNotFoundExceptionWhenInvalidIdOnDeleteIngredient()
    {
        //GIVEN
        var command = new DeleteIngredient
        {
            Id = Guid.NewGuid(),
            FamilyId = Guid.NewGuid(),
        };

        var commandHandler = new DeleteIngredientHandler(this.familyRepository, this.ingredientRepository, this.logger);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<FamilyNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.FAMILY_NOT_FOUND
                    && i.Id == command.FamilyId)
            ;
    }

    [TestMethod]
    public async Task ShouldThrowIngredientNotFoundExceptionWhenInvalidIdOnDeleteIngredient()
    {
        //GIVEN
        var command = new DeleteIngredient
        {
            Id = Guid.NewGuid(),
            FamilyId = this.familyId,
        };

        var commandHandler = new DeleteIngredientHandler(this.familyRepository, this.ingredientRepository, this.logger);

        //WHEN
        Func<Task<Unit>> action = async () => await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        await action.Should()
                .ThrowAsync<IngredientNotFoundException>()
                .Where(i =>
                    i.Code == ErrorCodes.INGREDIENT_NOT_FOUND
                    && i.Id == command.Id)
            ;
    }

    [TestMethod]
    public async Task ShouldDeleteIngredientOnDeleteIngredient()
    {
        //GIVEN
        var command = new DeleteIngredient
        {
            Id = this.ingredientId,
            FamilyId = this.familyId,
        };

        var commandHandler = new DeleteIngredientHandler(this.familyRepository, this.ingredientRepository, this.logger);

        //WHEN
        await commandHandler.Handle(command, CancellationToken.None);

        //THEN
        var ingredient = this.factory.Context.Ingredients.SingleOrDefault(i => i.Id == command.Id);
        ingredient.Should()
            .NotBeNull()
            ;
        ingredient!.IsActive.Should()
            .BeFalse()
            ;
    }

    public void Dispose()
    {
        this.factory.Dispose();
    }
}
