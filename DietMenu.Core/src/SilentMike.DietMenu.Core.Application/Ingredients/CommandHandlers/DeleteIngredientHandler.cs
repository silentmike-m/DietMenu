namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteIngredientHandler : IRequestHandler<DeleteIngredient>
{
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<DeleteIngredientHandler> logger;

    public DeleteIngredientHandler(IIngredientRepository ingredientRepository, ILogger<DeleteIngredientHandler> logger)
    {
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
    }

    public async Task<Unit> Handle(DeleteIngredient request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("IngredientId", request.Id)
        );

        this.logger.LogInformation("Try to delete ingredient");

        var ingredient = this.ingredientRepository.Get(request.FamilyId, request.Id);

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(request.Id);
        }

        ingredient.IsActive = false;

        this.ingredientRepository.Save(ingredient);

        this.ingredientRepository.SaveChanges();

        return await Task.FromResult(Unit.Value);
    }
}
