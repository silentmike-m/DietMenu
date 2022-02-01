namespace SilentMike.DietMenu.Core.Infrastructure.AutoMapper;

using global::AutoMapper;
using SourceMealType = SilentMike.DietMenu.Core.Domain.Entities.MealTypeEntity;
using TargetMealType = SilentMike.DietMenu.Core.Application.MealTypes.ViewModels.MealType;

internal sealed class MealTypeProfile : Profile
{
    public MealTypeProfile()
    {
        this.CreateMap<SourceMealType, TargetMealType>()
            .ForMember(target => target.Id, options => options.MapFrom(source => source.Id))
            .ForMember(target => target.Name, options => options.MapFrom(source => source.Name))
            .ForMember(target => target.Order, options => options.MapFrom(source => source.Order))
            ;
    }
}
