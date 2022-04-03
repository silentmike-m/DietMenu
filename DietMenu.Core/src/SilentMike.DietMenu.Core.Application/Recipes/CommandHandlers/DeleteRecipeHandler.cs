namespace SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Recipes;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.Events;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteRecipeHandler : IRequestHandler<DeleteRecipe>
{
    private readonly IFamilyRepository familyRepository;
    private readonly ILogger<DeleteRecipeHandler> logger;
    private readonly IMediator mediator;
    private readonly IRecipeRepository recipeRepository;

    public DeleteRecipeHandler(
        IFamilyRepository familyRepository,
        ILogger<DeleteRecipeHandler> logger,
        IMediator mediator,
        IRecipeRepository recipeRepository)
    {
        this.familyRepository = familyRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.recipeRepository = recipeRepository;
    }

    public async Task<Unit> Handle(DeleteRecipe request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("RecipeId", request.Id)
        );

        this.logger.LogInformation("Try to delete recipe");

        var family = await this.familyRepository.GetAsync(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var recipe = await this.recipeRepository.GetAsync(request.FamilyId, request.Id, cancellationToken);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(request.Id);
        }

        recipe.IsActive = false;

        await this.recipeRepository.SaveAsync(recipe, cancellationToken);

        var notification = new DeletedRecipe
        {
            FamilyId = request.FamilyId,
            Id = request.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
