using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RestaurantManagement.Models;

public partial class Restaurant
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ImgPath { get; set; } = null!;

    public virtual ICollection<Meal> Meals { get; } = new List<Meal>();
}
