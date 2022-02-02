namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceIngredient = SilentMike.DietMenu.Core.Domain.Entities.IngredientEntity;
using TargetIngredient = SilentMike.DietMenu.Core.Application.Ingredients.ViewModels.Ingredient;

public sealed class IngredientProfile : Profile
{
    public IngredientProfile()
    {
        this.CreateMap<SourceIngredient, TargetIngredient>()
            .ForMember(target => target.Id,
                opt => opt.MapFrom(source => source.Id))
            .ForMember(target => target.Exchanger,
                opt => opt.MapFrom(source => source.Exchanger))
            .ForMember(target => target.IsSystem,
                opt => opt.MapFrom(source => source.IsSystem))
            .ForMember(target => target.Name,
                opt => opt.MapFrom(source => source.Name))
            .ForMember(target => target.TypeId,
                opt => opt.MapFrom(source => source.TypeId))
            .ForMember(target => target.TypeName,
                opt => opt.MapFrom(source => source.Type.Name))
            .ForMember(target => target.UnitSymbol,
                opt => opt.MapFrom(source => source.UnitSymbol))
            ;
    }
}
