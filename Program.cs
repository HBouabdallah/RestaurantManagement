using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Models;
using RestaurantManagement.Repositories;
using RestaurantManagement.Repositories.Interfaces;
using RestaurantManagement.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<RestaurantManagementContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnection")));
        builder.Services.AddScoped<ICrudRepository<Restaurant>, RestaurantRepository>();
        builder.Services.AddScoped<ICrudRepository<Meal>, MealRepository>();
        builder.Services.AddSingleton<Endpoints>();
        builder.Services.AddHttpClient();

        builder.Services.AddScoped<IHttpService<Meal>,ThemealService>();
        builder.Services.AddControllers();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}