using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Repositories
{
    public class RestaurantRepository : CrudRepository<Restaurant>
    {
        private RestaurantManagementContext _context { get; }
        private Serilog.ILogger _logger { get; }

        public RestaurantRepository(RestaurantManagementContext context, Serilog.ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }


    }
}
