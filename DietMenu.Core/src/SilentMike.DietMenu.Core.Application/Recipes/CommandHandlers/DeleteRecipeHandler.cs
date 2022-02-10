namespace SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions.Recipes;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.Events;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteRecipeHandler : IRequestHandler<DeleteRecipe>
{
    private readonly ILogger<DeleteRecipeHandler> logger;
    private readonly IMediator mediator;
    private readonly IRecipeRepository repository;

    public DeleteRecipeHandler(ILogger<DeleteRecipeHandler> logger, IMediator mediator, IRecipeRepository repository)
        => (this.logger, this.mediator, this.repository) = (logger, mediator, repository);

    public async Task<Unit> Handle(DeleteRecipe request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("RecipeId", request.Id)
        );

        this.logger.LogInformation("Try to delete recipe");

        var recipe = await this.repository.Get(request.Id, cancellationToken);

        if (recipe is null)
        {
            throw new RecipeNotFoundException(request.Id);
        }

        await this.repository.Delete(recipe, cancellationToken);

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
