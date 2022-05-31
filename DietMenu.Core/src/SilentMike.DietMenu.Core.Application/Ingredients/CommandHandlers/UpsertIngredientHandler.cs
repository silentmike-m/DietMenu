namespace SilentMike.DietMenu.Core.Application.Ingredients.CommandHandlers;

using FluentValidation.Results;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.IngredientTypes;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Ingredients.Commands;
using SilentMike.DietMenu.Core.Application.Ingredients.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertIngredientHandler : IRequestHandler<UpsertIngredient>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<UpsertIngredientHandler> logger;
    private readonly IIngredientTypeRepository typeRepository;

    public UpsertIngredientHandler(IFamilyRepository familyRepository, IIngredientRepository ingredientRepository, ILogger<UpsertIngredientHandler> logger, IIngredientTypeRepository typeRepository)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
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

        var family = this.familyRepository.Get(request.FamilyId);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var ingredient = this.ingredientRepository.Get(request.FamilyId, request.Ingredient.Id);

        if (ingredient is null)
        {
            ingredient = this.Create(request.FamilyId, request.Ingredient);
        }
        else
        {
            this.Update(request.FamilyId, ingredient, request.Ingredient);
        }

        this.ingredientRepository.Save(ingredient);

        return await Task.FromResult(Unit.Value);
    }

    private IngredientEntity Create(Guid familyId, IngredientToUpsert ingredientToUpsert)
    {
        this.logger.LogInformation("Try to create ingredient");

        ValidateNewIngredient(ingredientToUpsert);

        this.ValidateIngredientType(familyId, ingredientToUpsert.TypeId);

        var ingredient = new IngredientEntity(ingredientToUpsert.Id)
        {
            Exchanger = ingredientToUpsert.Exchanger!.Value,
            FamilyId = familyId,
            InternalName = ingredientToUpsert.Id.ToString(),
            Name = ingredientToUpsert.Name!,
            TypeId = ingredientToUpsert.TypeId!.Value,
            UnitSymbol = ingredientToUpsert.UnitSymbol!,
        };

        return ingredient;
    }

    private void Update(Guid familyId, IngredientEntity ingredient, IngredientToUpsert ingredientToUpsert)
    {
        this.logger.LogInformation("Try to update ingredient");

        this.ValidateIngredientType(familyId, ingredientToUpsert.TypeId);

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

        if (ingredientToUpsert.TypeId is null)
        {
            errors.Add(new ValidationFailure(nameof(ingredientToUpsert.TypeId), ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_TYPE_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_INGREDIENT_EMPTY_TYPE,
            });
        }

        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }

    private void ValidateIngredientType(Guid familyId, Guid? typeId)
    {
        if (typeId is null)
        {
            return;
        }

        var type = this.typeRepository.Get(familyId, typeId.Value);

        if (type is null)
        {
            throw new IngredientTypeNotFoundException(typeId.Value);
        }
    }
}
