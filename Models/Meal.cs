using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RestaurantManagement.Models;

public partial class Meal
{
    [Newtonsoft.Json.JsonProperty("idMeal")]
    public int Id { get; set; }

    [Newtonsoft.Json.JsonProperty("strMeal")]
    public string Name { get; set; } = null!;

    [Newtonsoft.Json.JsonProperty("strMealThumb")]
    public string ImgPath { get; set; } = null!;

    [Newtonsoft.Json.JsonProperty("strCategory")]
    public string Category { get; set; } = null!;

    public string Ingredient { get; set; } = null!;

    public string Quantity { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Restaurant> Restaurants { get; } = new List<Restaurant>();
}
