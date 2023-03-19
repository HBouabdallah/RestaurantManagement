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
        // GET: api/<RestaurantController>
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

        [HttpGet("GetMealsOfRestaurant{RestaurantId}")]
        public async Task<IActionResult> GetMealsOfRestaurant(int RestaurantId)
        {
            var restaurant = await _restaurantRepository.GetByFilterAsync((x => x.Id == RestaurantId), (x => x.Meals));

            if (restaurant != null)
            {
                return Ok(restaurant.Meals);
            }
            else
            {
                return StatusCode(500, "Restaurant not exist");
            }
        }

        // POST api/<RestaurantController>
        [HttpPost("AddNewRestaurant")]
        public async Task<IActionResult> Post([FromBody] RestaurantMealDto restaurantDto)
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

        [HttpPost("AddMealsToRestaurant{restaurantId}")]
        public async Task<IActionResult> Post([FromBody] List<int> listMealId, int restaurantId)
        {

            var meals = await _mealRepository.GetListByFilterAsync(x => listMealId.Contains(x.Id));
            if (meals.Count() < 1)
            {
                foreach (var mealId in listMealId)
                {
                    meals.Add(await _mealService.GetById(_endpoints.GetMealById, mealId));
                }
            }
            if (meals.Count() < 1)
            {
                return NotFound();
            }
            var restaurant = await _restaurantRepository.GetByFilterAsync(x => x.Id == restaurantId, x => x.Meals);
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
