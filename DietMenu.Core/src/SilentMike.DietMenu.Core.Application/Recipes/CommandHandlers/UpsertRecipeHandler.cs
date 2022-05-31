namespace SilentMike.DietMenu.Core.Application.Recipes.CommandHandlers;

using FluentValidation.Results;
using SilentMike.DietMenu.Core.Application.Exceptions;
using SilentMike.DietMenu.Core.Application.Exceptions.Families;
using SilentMike.DietMenu.Core.Application.Exceptions.Ingredients;
using SilentMike.DietMenu.Core.Application.Exceptions.MealTypes;
using SilentMike.DietMenu.Core.Application.Extensions;
using SilentMike.DietMenu.Core.Application.Recipes.Commands;
using SilentMike.DietMenu.Core.Application.Recipes.ValueModels;
using SilentMike.DietMenu.Core.Domain.Entities;
using SilentMike.DietMenu.Core.Domain.Repositories;

internal sealed class UpsertRecipeHandler : IRequestHandler<UpsertRecipe>
{
    private readonly IFamilyRepository familyRepository;
    private readonly IIngredientRepository ingredientRepository;
    private readonly ILogger<UpsertRecipeHandler> logger;
    private readonly IMealTypeRepository mealTypeRepository;
    private readonly IRecipeRepository recipeRepository;

    public UpsertRecipeHandler(IFamilyRepository familyRepository, IIngredientRepository ingredientRepository, ILogger<UpsertRecipeHandler> logger, IMealTypeRepository mealTypeRepository, IRecipeRepository recipeRepository)
    {
        this.familyRepository = familyRepository;
        this.ingredientRepository = ingredientRepository;
        this.logger = logger;
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

        var family = this.familyRepository.Get(request.FamilyId);

        if (family is null)
        {
            throw new FamilyNotFoundException(request.FamilyId);
        }

        var recipe = this.recipeRepository.Get(request.FamilyId, request.Recipe.Id);

        if (recipe is null)
        {
            this.Create(request.FamilyId, request);
        }
        else
        {
            this.Update(request.FamilyId, recipe, request.Recipe);
        }

        return await Task.FromResult(Unit.Value);
    }

    private void Create(Guid familyId, UpsertRecipe request)
    {
        this.logger.LogInformation("Try to create recipe");

        ValidateNewRecipe(request.Recipe);

        this.ValidateMealType(familyId, request.Recipe.MealTypeId);

        var recipeIngredients = this.CreateIngredients(familyId, request.Recipe);

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

        this.recipeRepository.Save(recipe);
    }

    private void Update(Guid familyId, RecipeEntity recipe, RecipeToUpsert recipeToUpsert)
    {
        this.logger.LogInformation("Try to update recipe");

        this.ValidateMealType(familyId, recipeToUpsert.MealTypeId);

        recipe.Carbohydrates = recipeToUpsert.Carbohydrates ?? recipe.Carbohydrates;
        recipe.Description = recipeToUpsert.Description ?? recipe.Description;
        recipe.Energy = recipeToUpsert.Energy ?? recipe.Energy;
        recipe.Fat = recipeToUpsert.Fat ?? recipe.Fat;
        recipe.Ingredients = this.CreateIngredients(familyId, recipeToUpsert);
        recipe.MealTypeId = recipeToUpsert.MealTypeId ?? recipe.MealTypeId;
        recipe.Name = recipeToUpsert.Name ?? recipe.Name;
        recipe.Protein = recipeToUpsert.Protein ?? recipe.Protein;

        this.recipeRepository.Save(recipe);
    }

    private List<RecipeIngredientEntity> CreateIngredients(Guid familyId, RecipeToUpsert recipeToUpsert)
    {
        var recipeIngredients = new List<RecipeIngredientEntity>();

        foreach (var ingredientToUpsert in recipeToUpsert.Ingredients)
        {
            var ingredient = this.ingredientRepository.Get(familyId, ingredientToUpsert.IngredientId);

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

    private void ValidateMealType(Guid familyId, Guid? mealTypeId)
    {
        if (mealTypeId.HasValue)
        {
            var mealType = this.mealTypeRepository.Get(familyId, mealTypeId.Value);

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
