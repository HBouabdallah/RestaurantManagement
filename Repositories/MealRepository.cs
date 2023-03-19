using RestaurantManagement.Models;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Repositories
{
    public class MealRepository : CrudRepository<Meal>
    {
        public RestaurantManagementContext _context { get; }
        public Serilog.ILogger _logger { get; }

        public MealRepository(RestaurantManagementContext context, Serilog.ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

    }
}
