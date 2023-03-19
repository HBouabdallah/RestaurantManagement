using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Repositories;
using RestaurantManagement.Repositories.Interfaces;

namespace RestaurantManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MealController : ControllerBase
    {
        private ICrudRepository<Meal> _mealRepository { get; }
        private IMapper _mapper { get; }

        public MealController(ICrudRepository<Meal> mealRepository, IMapper mapper)
        {
            _mealRepository = mealRepository;
            _mapper = mapper;
        }
        /* Une méthode qui repond au besoin: Afficher une vue détaillée d’un plat (Nom, Image, Catégorie, Ingrédients et quantités)
        */
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
