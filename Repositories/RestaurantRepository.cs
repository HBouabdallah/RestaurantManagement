using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Repositories
{
    public class RestaurantRepository : CrudRepository<Restaurant>
    {
        public RestaurantManagementContext _context { get; }

        public RestaurantRepository(RestaurantManagementContext context) : base(context)
        {
            _context = context;
        }


    }
}
