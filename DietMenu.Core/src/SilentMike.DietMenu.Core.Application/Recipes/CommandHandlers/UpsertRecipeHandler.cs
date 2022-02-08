namespace SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using SilentMike.DietMenu.Core.Application.Common;
using SilentMike.DietMenu.Core.Application.Common.Constants;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Exceptions.MealTypes;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.Events;
using SilentMike.DietMenu.Core.Application.Recipes.ViewModels.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertRecipeHandler : IRequestHandler<UpsertRecipe>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<UpsertRecipeHandler> logger;
    private readonly IMediator mediator;
    private readonly IMealTypeRepository mealTypeRepository;
    private readonly IRecipeRepository recipeRepository;

    public UpsertRecipeHandler(
        IFamilyRepository familyRepository,
        IIngredientRepository ingredientRepository,
        ILogger<UpsertRecipeHandler> logger,
        IMediator mediator,
        IMealTypeRepository mealTypeRepository,
        IRecipeRepository recipeRepository)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
        this.mediator = mediator;
        this.mealTypeRepository = mealTypeRepository;
        this.recipeRepository = recipeRepository;
    }

    public async Task<Unit> Handle(UpsertRecipe request, CancellationToken cancellationToken)
    {
        using var loggerScope = this.logger.BeginPropertyScope(
            ("FamilyId", request.FamilyId),
            ("UserId", request.UserId),
            ("RecipeId", request.Recipe.Id)
        );

        this.logger.LogInformation("Try to upsert recipe");

        var family = await this.familyRepository.Get(request.FamilyId, cancellationToken);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var recipe = await this.recipeRepository.Get(request.Recipe.Id, CancellationToken.None);

        if (recipe is null)
        {
            await this.Create(request, cancellationToken);
        }
        else
        {
            await this.Update(recipe, request.Recipe, cancellationToken);
        }

        var notification = new UpsertedRecipe
        {
            FamilyId = request.FamilyId,
            Id = request.Recipe.Id,
            UserId = request.UserId,
        };

        await this.mediator.Publish(notification, cancellationToken);

        return await Task.FromResult(Unit.Value);
    }

    private async Task Create(UpsertRecipe request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to create recipe");

        ValidateNewRecipe(request.Recipe);

        await this.ValidateMealType(request.Recipe.MealTypeId, cancellationToken);

        var recipeIngredients = await this.CreateIngredients(request.Recipe, cancellationToken);

        var recipe = new RecipeEntity(request.Recipe.Id)
        {
            Carbohydrates = request.Recipe.Carbohydrates ?? 0,
            Description = request.Recipe.Description ?? string.Empty,
            Energy = request.Recipe.Energy ?? 0,
            FamilyId = request.FamilyId,
            Fat = request.Recipe.Fat ?? 0,
            Ingredients = recipeIngredients,
            MealTypeId = request.Recipe.MealTypeId ?? Guid.Empty,
            Name = request.Recipe.Name ?? string.Empty,
            Protein = request.Recipe.Protein ?? 0,
            UserId = request.UserId,
        };

        await this.recipeRepository.Save(recipe, cancellationToken);
    }

    private async Task Update(RecipeEntity recipe, RecipeToUpsert recipeToUpsert, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Try to update recipe");

        await this.ValidateMealType(recipeToUpsert.MealTypeId, cancellationToken);

        recipe.Carbohydrates = recipeToUpsert.Carbohydrates ?? recipe.Carbohydrates;
        recipe.Description = recipeToUpsert.Description ?? recipe.Description;
        recipe.Energy = recipeToUpsert.Energy ?? recipe.Energy;
        recipe.Fat = recipeToUpsert.Fat ?? recipe.Fat;
        recipe.Ingredients = await this.CreateIngredients(recipeToUpsert, cancellationToken);
        recipe.MealTypeId = recipeToUpsert.MealTypeId ?? recipe.MealTypeId;
        recipe.Name = recipeToUpsert.Name ?? recipe.Name;
        recipe.Protein = recipeToUpsert.Protein ?? recipe.Protein;

        await this.recipeRepository.Save(recipe, cancellationToken);
    }

    private async Task<List<RecipeIngredientEntity>> CreateIngredients(RecipeToUpsert recipeToUpsert, CancellationToken cancellationToken)
    {
        var recipeIngredients = new List<RecipeIngredientEntity>();

        foreach (var ingredientToUpsert in recipeToUpsert.Ingredients)
        {
            var ingredient = await this.ingredientRepository.Get(ingredientToUpsert.IngredientId, cancellationToken);

            if (ingredient is null)
            {
                throw new IngredientNotFoundException(ingredientToUpsert.IngredientId);
            }

            var recipeIngredient = new RecipeIngredientEntity(ingredientToUpsert.Id)
            {
                IngredientId = ingredient.Id,
                Quantity = ingredientToUpsert.Quantity,
                RecipeId = recipeToUpsert.Id,
            };

            recipeIngredients.Add(recipeIngredient);
        }

        return recipeIngredients;
    }

    private async Task ValidateMealType(Guid? mealTypeId, CancellationToken cancellationToken)
    {
        if (mealTypeId.HasValue)
        {
            var mealType = await this.mealTypeRepository.Get(mealTypeId.Value, cancellationToken);

            if (mealType is null)
            {
                throw new MealTypeNotFoundException(mealTypeId.Value);
            }
        }
    }

    private static void ValidateNewRecipe(RecipeToUpsert recipeToUpsert)
    {
        var errors = new List<ValidationFailure>();

        if (recipeToUpsert.Carbohydrates is null)
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Name), ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_INVALID_CARBOHYDRATES,
            });
        }

        if (recipeToUpsert.Energy is null)
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Energy), ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_INVALID_ENERGY,
            });
        }

        if (recipeToUpsert.Fat is null)
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Fat), ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_INVALID_FAT,
            });
        }

        if (recipeToUpsert.MealTypeId is null)
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Fat), ValidationErrorCodes.UPSERT_RECIPE_EMPTY_MEAL_TYPE_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_EMPTY_MEAL_TYPE,
            });
        }

        if (string.IsNullOrWhiteSpace(recipeToUpsert.Name))
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Name), ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_EMPTY_NAME,
            });
        }

        if (recipeToUpsert.Protein is null)
        {
            errors.Add(new ValidationFailure(nameof(recipeToUpsert.Protein), ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN_MESSAGE)
            {
                ErrorCode = ValidationErrorCodes.UPSERT_RECIPE_INVALID_PROTEIN,
            });
        }


        if (errors.Any())
        {
            throw new ValidationException(errors);
        }
    }
}
