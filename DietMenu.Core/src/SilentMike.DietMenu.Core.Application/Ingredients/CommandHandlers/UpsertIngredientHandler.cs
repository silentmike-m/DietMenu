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

internal sealed class UpsertIngredientHandler : IRequestHandler<UpsertIngredient>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<UpsertIngredientHandler> logger;
    private readonly IMediator mediator;
    private readonly IIngredientTypeRepository typeRepository;

    public UpsertIngredientHandler(
        IFamilyRepository familyRepository,
        IIngredientRepository ingredientRepository,
        ILogger<UpsertIngredientHandler> logger,
        IMediator mediator,
        IIngredientTypeRepository typeRepository)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.typeRepository = typeRepository;
    }

    public async Task<Unit> Handle(UpsertIngredient request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("IngredientId", request.Ingredient.Id)
        );

        this.logger.LogInformation("Try to upsert ingredient");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var ingredient = await this.ingredientRepository.Get(request.Ingredient.Id, cancellationToken);

        if (ingredient is null)
        {
            ingredient = await this.Create(request.FamilyId, request.Ingredient, cancellationToken);
        }
        else
        {
            await this.Update(ingredient, request.Ingredient, cancellationToken);
        }

        await this.ingredientRepository.Save(ingredient, cancellationToken);

        var notification = new UpsertedIngredient
        {
            FamilyId = request.FamilyId,
            Id = ingredient.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private async Task<IngredientEntity> Create(Guid familyId, IngredientToUpsert ingredientToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to create ingredient");

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

        return ingredient;
    }

    private async Task Update(IngredientEntity ingredient, IngredientToUpsert ingredientToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to update ingredient");

        await this.ValidateIngredientType(ingredientToUpsert.TypeId, cancellationToken);

        ingredient.Exchanger = ingredientToUpsert.Exchanger ?? ingredient.Exchanger;
        ingredient.Name = ingredientToUpsert.Name ?? ingredient.Name;
        ingredient.TypeId = ingredientToUpsert.TypeId ?? ingredient.TypeId;
        ingredient.UnitSymbol = ingredientToUpsert.UnitSymbol ?? ingredient.UnitSymbol;
    }

    private static void ValidateNewIngredient(IngredientToUpsert ingredientToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (ingredientToUpsert.Exchanger is null)
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_INVALID_EXCHANGER,
            });
        }

        if (string.IsNullOrWhiteSpace(ingredientToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_NAME,
            });
        }

        if (string.IsNullOrWhiteSpace(ingredientToUpsert.UnitSymbol))
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.Name), ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_UNIT,
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
