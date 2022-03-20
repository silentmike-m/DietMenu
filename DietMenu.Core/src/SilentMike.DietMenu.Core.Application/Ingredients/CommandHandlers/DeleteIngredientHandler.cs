namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class DeleteIngredientHandler : IRequestHandler<DeleteIngredient>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<DeleteIngredientHandler> logger;
    private readonly IMediator mediator;

    public DeleteIngredientHandler(
        IFamilyRepository familyRepository,
        IIngredientRepository ingredientRepository,
        ILogger<DeleteIngredientHandler> logger,
        IMediator mediator)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteIngredient request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("IngredientId", request.Id)
        );

        this.logger.LogInformation("Try to delete ingredient");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var ingredient = await this.ingredientRepository.Get(request.FamilyId, request.Id, cancellationToken);

        if (ingredient is null)
        {
            throw new IngredientNotFoundException(request.Id);
        }

        ingredient.IsActive = false;

        await this.ingredientRepository.Save(ingredient, cancellationToken);

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
