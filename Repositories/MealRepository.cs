using RestaurantManagement.Models;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Repositories
{
    public class MealRepository : CrudRepository<Meal>
    {
        public RestaurantManagementContext _context { get; }
        public MealRepository(RestaurantManagementContext context) : base(context)
        {
            _context = context;
        }

    }
}
