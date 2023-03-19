using AutoMapper;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;

namespace RestaurantManagement.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RestaurantDto, Restaurant>();
            CreateMap<Meal, MealDto>();
        }
    }
}
