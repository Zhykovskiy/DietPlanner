using DietPlanner.Models.Meal;

namespace DietPlanner.Mappers;

public class MappersProfile : AutoMapper.Profile
{
    public MappersProfile()
    {
        CreateMap<AddMealToPlanViewModel, AddToMealPlanRequest>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => (int)src.Date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds))
            .ReverseMap();
    }
}
