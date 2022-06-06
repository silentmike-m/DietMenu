namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceMealType = SilentMike.DietMenu.Core.Infrastructure.EntityFramework.Models.MealTypeRow;
using TargetMealType = SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.MealType;

internal sealed class MealTypeRowProfile : Profile
{
    public MealTypeRowProfile()
    {
        this.CreateMap<SourceMealType, TargetMealType>()
            .ForMember(target => target.Id,
                opt => opt.MapFrom(source => source.Id))
            .ForMember(target => target.Name,
                opt => opt.MapFrom(source => source.Name))
            .ForMember(target => target.Order,
                opt => opt.MapFrom(source => source.Order))
            ;
    }
}
