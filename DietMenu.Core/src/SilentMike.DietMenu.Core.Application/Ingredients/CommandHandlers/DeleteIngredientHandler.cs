namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteIngredientHandler : IRequestHandler<DeleteIngredient>
{
    private readonly ILogger<DeleteIngredientHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientRepository repository;

    public DeleteIngredientHandler(ILogger<DeleteIngredientHandler> logger, IMediator mediator, IIngredientRepository repository)
        => (this.logger, this.mediator, this.repository) = (logger, mediator, repository);

    public async Task<Unit> Handle(DeleteIngredient request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("IngredientId", request.Id)
        );

        this.logger.LogInformation("Try to delete ingredient");

        var ingredient = await this.repository.Get(request.Id, cancellationToken);

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(request.Id);
        }

        if (ingredient.IsSystem)
        {
            throw new DeleteSystemValueException(request.Id, nameof(IngredientEntity));
        }

        await this.repository.Delete(ingredient, cancellationToken);

        var notification = new DeletedIngredient
        {
            FamilyId = request.FamilyId,
            Id = request.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }
}
