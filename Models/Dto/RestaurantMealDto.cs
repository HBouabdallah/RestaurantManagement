using Newtonsoft.Json;

namespace RestaurantManagement.Models.Dto
{
    public class RestaurantMealDto
    {
        public string Name { get; set; } = null!;

        public string ImgPath { get; set; } = null!;
        public List<Meal> Meals { get; set; } = new List<Meal>();
    }
}
