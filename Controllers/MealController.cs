using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        public ICrudRepository<Meal> _mealRepository { get; }
        public MealController(ICrudRepository<Meal> mealRepository)
        {
            _mealRepository = mealRepository;
        }

        [HttpGet("GetMealById{mealId}")]
        public async Task<IActionResult> GetMealById(int mealId)
        {
            var meal = await _mealRepository.GetByFilterAsync((x => x.Id == mealId));

            if (meal != null)
            {
                return Ok(meal);
            }
            else
            {
                return StatusCode(500, "meal not exist");
            }
        }
    }
}
