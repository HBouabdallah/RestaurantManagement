using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Repositories.Interfaces;
using RestaurantManagement.Services;

namespace RestaurantManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private ICrudRepository<Restaurant> _restaurantRepository { get; }
        private ICrudRepository<Meal> _mealRepository { get; }
        private IHttpService<Meal> _mealService { get; }
        private IMapper _mapper { get; }
        public Endpoints _endpoints { get; }

        public RestaurantController(ICrudRepository<Restaurant> repository, ICrudRepository<Meal> mealRepository, IHttpService<Meal> mealService, IMapper mapper, Endpoints endpoints)
        {
            _restaurantRepository = repository;
            _mealRepository = mealRepository;
            _mealService = mealService;
            _mapper = mapper;
            _endpoints = endpoints;
        }
        /* Une méthode qui repond au besoin: Affichage de la liste des restaurants - Fonctionnalité 1 */

        [HttpGet("GetAllRestaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantRepository.GetAllAsync(x => x.Meals);
            if (restaurants != null)
            {
                return Ok(restaurants);
            }
            else
            {
                return StatusCode(500, "Error while getting restaurants");
            }
        }
        /* Une méthode qui repond au besoin: Afficher les plats disponibles dans un restaurant (Nom + Image) - Fonctionnalité 3
         cette méthode utilise un DTO pour formater la réponse */

        [HttpGet("GetMealsOfRestaurant{RestaurantId}")]
        public async Task<IActionResult> GetMealsOfRestaurant(int RestaurantId)
        {
            var restaurant = await _restaurantRepository.GetByFilterAsync((x => x.Id == RestaurantId), (x => x.Meals));

            if (restaurant != null)
            {
                var meamDto = _mapper.Map<List<MealDto>>(restaurant.Meals);

                return Ok(meamDto);
            }
            else
            {
                return StatusCode(500, "Restaurant not exist");
            }
        }
        /* Une méthode qui repond au besoin: Ajouter de nouveaux restaurants */

        [HttpPost("AddNewRestaurant")]
        public async Task<IActionResult> AddNewRestaurant([FromBody] RestaurantDto restaurantDto)
        {
            if (restaurantDto == null)
            {
                return BadRequest(ModelState);
            }

            var restaurant = _mapper.Map<Restaurant>(restaurantDto);
            var isAdded = await _restaurantRepository.AddEntityAsync(restaurant);

            if (isAdded)
            {
                return Ok("Successfully created");
            }
            else
            {
                return StatusCode(500, "Error while saving");
            }

        }
        /* Une méthode qui repond au besoin: Pouvoir ajouter de nouveaux plats dans le menu d’un restaurant grâce à des propositions de l’API TheMealDb.
         cette méthode prend en paramètre la liste des identifiants des plats à ajouter et l'identifiant du restaurant.
          1 - nous vérifions si un repas existe déjà dans notre base de données par ID
          2 - s'il existe donc nous n'avons pas besoin de faire un appel à l'API sinon nous faisons l'appel
          3 - on récupère le plat depuis l'API et on l'ajoute à la liste des plats
          4 - on récupère le restaurant par ID
          5- si le restaurant existe on met à jour la liste des plats et on fait la mise à jour sinon on retourne notfound */

        [HttpPost("AddMealsToRestaurant{restaurantId}")]
        public async Task<IActionResult> AddMealsToRestaurant([FromBody] List<int> listMealId, int restaurantId)
        {
            List<Meal> meals = new List<Meal>();
            foreach (var mealId in listMealId)
            {
                var DBmeal = await _mealRepository.GetByFilterAsync(x => mealId == x.Id);
                if (DBmeal != null)
                {
                    meals.Add(DBmeal);
                }
                else
                {
                    var meal = await _mealService.GetById(_endpoints.GetMealById, mealId);
                    if (meal != null)
                    {
                        meals.Add(meal);
                    }
                }
            }

            if (meals.Count() < 1)
            {
                return NotFound();
            }
            var restaurant = await _restaurantRepository.GetByFilterAsync(x => x.Id == restaurantId, x => x.Meals);
            if (restaurant == null)
            {
                return NotFound();
            }
            foreach (Meal meal in meals)
            {
                if (!restaurant.Meals.Contains(meal))
                    restaurant.Meals.Add(meal);
            }
            await _restaurantRepository.UpdateEntityAsync(restaurant);

            return Ok("Meals list updated succesfully"); ;
        }
    }
}
