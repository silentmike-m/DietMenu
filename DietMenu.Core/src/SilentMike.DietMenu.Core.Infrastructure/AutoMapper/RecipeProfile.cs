namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceRecipe = SilentMike.DietMenu.Core.Domain.Entities.RecipeEntity;
using SourceRecipeIngredient = SilentMike.DietMenu.Core.Domain.Entities.RecipeIngredientEntity;
using TargetRecipe = SilentMike.DietMenu.Core.Application.Recipes.ViewModels.Recipe;
using TargetRecipeIngredient = SilentMike.DietMenu.Core.Application.Recipes.ViewModels.RecipeIngredient;

public sealed class RecipeProfile : Profile
{
    public RecipeProfile()
    {
        this.CreateMap<SourceRecipe, TargetRecipe>()
            .ForMember(target => target.Id,
                opt => opt.MapFrom(source => source.Id))
            .ForMember(target => target.Carbohydrates,
                opt => opt.MapFrom(source => source.Carbohydrates))
            .ForMember(target => target.Description,
                opt => opt.MapFrom(source => source.Description))
            .ForMember(target => target.Energy,
                opt => opt.MapFrom(source => source.Energy))
            .ForMember(target => target.Fat,
                opt => opt.MapFrom(source => source.Fat))
            .ForMember(target => target.MealTypeId,
                opt => opt.MapFrom(source => source.MealTypeId))
            .ForMember(target => target.MealTypeName,
                opt => opt.MapFrom(source => source.MealType.Name))
            .ForMember(target => target.Name,
                opt => opt.MapFrom(source => source.Name))
            .ForMember(target => target.Protein,
                opt => opt.MapFrom(source => source.Protein))
            ;

        this.CreateMap<SourceRecipeIngredient, TargetRecipeIngredient>()
            .ForMember(target => target.Id,
                opt => opt.MapFrom(source => source.Id))
            .ForMember(target => target.IngredientExchanger,
                opt => opt.MapFrom(source => source.Ingredient.Exchanger))
            .ForMember(target => target.IngredientId,
                opt => opt.MapFrom(source => source.IngredientId))
            .ForMember(target => target.IngredientName,
                opt => opt.MapFrom(source => source.Ingredient.Name))
            .ForMember(target => target.IngredientTypeId,
                opt => opt.MapFrom(source => source.Ingredient.TypeId))
            .ForMember(target => target.IngredientTypeName,
                opt => opt.MapFrom(source => source.Ingredient.Type.Name))
            .ForMember(target => target.Quantity,
                opt => opt.MapFrom(source => source.Quantity))
            .ForMember(target => target.UnitSymbol,
                opt => opt.MapFrom(source => source.Ingredient.UnitSymbol))
            ;
    }
}
