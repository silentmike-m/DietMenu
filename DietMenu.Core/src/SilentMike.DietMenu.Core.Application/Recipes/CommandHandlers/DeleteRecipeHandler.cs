namespace SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;

using SilentMike.DietMenu.Core.Application.Exceptions.Recipes;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteRecipeHandler : IRequestHandler<DeleteRecipe>
{
    private readonly ILogger<DeleteRecipeHandler> logger;
    private readonly IRecipeRepository recipeRepository;

    public DeleteRecipeHandler(ILogger<DeleteRecipeHandler> logger, IRecipeRepository recipeRepository)
        => (this.logger, this.recipeRepository) = (logger, recipeRepository);

    public async Task<Unit> Handle(DeleteRecipe request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("RecipeId", request.Id)
        );

        this.logger.LogInformation("Try to delete recipe");

        var recipe = this.recipeRepository.Get(request.FamilyId, request.Id);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(request.Id);
        }

        recipe.IsActive = false;

        this.recipeRepository.Save(recipe);

        this.recipeRepository.SaveChanges();

        return await Task.FromResult(Unit.Value);
    }
}
