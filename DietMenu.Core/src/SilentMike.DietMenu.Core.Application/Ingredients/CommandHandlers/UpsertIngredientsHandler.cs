namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.Events;
using SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertIngredientsHandler : IRequestHandler<UpsertIngredients>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<UpsertIngredientsHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientTypeRepository typeRepository;

    public UpsertIngredientsHandler(
        IFamilyRepository familyRepository,
        IIngredientRepository ingredientRepository,
        ILogger<UpsertIngredientsHandler> logger,
        IMediator mediator,
        IIngredientTypeRepository typeRepository)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.typeRepository = typeRepository;
    }

    public async Task<Unit> Handle(UpsertIngredients request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId)
        );

        this.logger.LogInformation("Try to upsert ingredients");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        foreach (var ingredientToUpsert in request.Ingredients)
        {
            var ingredient = await this.ingredientRepository.Get(ingredientToUpsert.Id, cancellationToken);

            if (ingredient is null)
            {
                await this.Create(request.FamilyId, ingredientToUpsert, cancellationToken);
            }
            else
            {
                await this.Update(ingredient, ingredientToUpsert, cancellationToken);
            }
        }

        var ids = request.Ingredients
            .Select(i => i.Id)
            .ToList();

        var notification = new UpsertedIngredients
        {
            FamilyId = request.FamilyId,
            Ids = ids.AsReadOnly(),
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private async Task Create(Guid familyId, IngredientToUpsert ingredientToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to create ingredient with id {IngredientId}", ingredientToUpsert.Id);

        await this.ValidateIngredientType(ingredientToUpsert.TypeId, cancellationToken);

        ValidateNewIngredient(ingredientToUpsert);

        var ingredient = new IngredientEntity(ingredientToUpsert.Id)
        {
            Exchanger = ingredientToUpsert.Exchanger!.Value,
            FamilyId = familyId,
            InternalName = ingredientToUpsert.Id.ToString(),
            IsSystem = false,
            Name = ingredientToUpsert.Name!,
            TypeId = ingredientToUpsert.TypeId!.Value,
            UnitSymbol = ingredientToUpsert.UnitSymbol!,
        };

        await this.ingredientRepository.Save(ingredient, cancellationToken);
    }

    private async Task Update(IngredientEntity ingredient, IngredientToUpsert ingredientToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to update ingredient with id {IngredientId}", ingredientToUpsert.Id);

        await this.ValidateIngredientType(ingredientToUpsert.TypeId, cancellationToken);

        ingredient.Exchanger = ingredientToUpsert.Exchanger ?? ingredient.Exchanger;
        ingredient.Name = ingredientToUpsert.Name ?? ingredient.Name;
        ingredient.TypeId = ingredientToUpsert.TypeId ?? ingredient.TypeId;
        ingredient.UnitSymbol = ingredientToUpsert.UnitSymbol ?? ingredient.UnitSymbol;

        await this.ingredientRepository.Save(ingredient, cancellationToken);
    }

    private static void ValidateNewIngredient(IngredientToUpsert ingredientToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (ingredientToUpsert.Exchanger is null)
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENTS_INVALID_EXCHANGER,
            });
        }

        if (string.IsNullOrWhiteSpace(ingredientToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_NAME,
            });
        }

        if (string.IsNullOrWhiteSpace(ingredientToUpsert.UnitSymbol))
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENTS_EMPTY_UNIT,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }

    private async Task ValidateIngredientType(Guid? typeId, CancellationToken cancellationToken)
    {
        if (typeId is null)
        {
            return;
        }

        var type = await this.typeRepository.Get(typeId.Value, cancellationToken);

        if (type is null)
        {
            throw new IngredientTypeNotFoundException(typeId.Value);
        }
    }
}
