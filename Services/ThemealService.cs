using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestaurantManagement.Models;

namespace RestaurantManagement.Services
{
    public class ThemealService : IHttpService<Meal>
    {
        private HttpClient _client { get; }
        private Serilog.ILogger _logger { get; }

        public ThemealService(HttpClient client, Serilog.ILogger logger)
        {
            _client = client;
            _logger = logger;
        }


        public async Task<Meal> GetById(string endpoint, int id)
        {

            var builder = new UriBuilder(endpoint);

            using (var client = new HttpClient())
            {
                builder.Query = "i=" + id.ToString();


                HttpResponseMessage response = await client.GetAsync(builder.Uri);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var jsonObject = JObject.Parse(result);
                        var meals = jsonObject["meals"][0];
                        var ingerdients = "";
                        var quantity = "";
                        for (int i = 0; i < 20; i++)
                        {
                            ingerdients += (meals["strIngredient" + i] != null ? meals["strIngredient" + i].ToString() != "" ? meals["strIngredient" + i].ToString() + "," : "" : "");
                            quantity += (meals["strMeasure" + i] != null ? meals["strMeasure" + i].ToString() != "" ? meals["strMeasure" + i].ToString() + "," : "" : "");
                        }
                        var meal = JsonConvert.DeserializeObject<Meal>(meals.ToString());
                        meal.Ingredient = ingerdients;
                        meal.Quantity = quantity;
                        return (meal ?? null);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(ex.Message);
                    }
                }
                else
                {
                    _logger.Error(response.StatusCode.ToString());
                }
                return null;
            }
        }

    }
}
