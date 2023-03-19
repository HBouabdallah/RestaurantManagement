using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RestaurantManagement.Models;

public partial class RestaurantManagementContext : DbContext
{
    public RestaurantManagementContext()
    {
    }

    public RestaurantManagementContext(DbContextOptions<RestaurantManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Meal> Meal { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("db_owner");

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Category).HasMaxLength(250);
            entity.Property(e => e.Ingredient).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<Restaurant>(entity =>
        {
            entity.ToTable("Restaurant");

            entity.Property(e => e.Name).HasMaxLength(250);

            entity.HasMany(d => d.Meals).WithMany(p => p.Restaurants)
                .UsingEntity<Dictionary<string, object>>(
                    "RestaurantMeal",
                    r => r.HasOne<Meal>().WithMany()
                        .HasForeignKey("IdMeal")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RestaurantMeal_Meal"),
                    l => l.HasOne<Restaurant>().WithMany()
                        .HasForeignKey("IdRestaurant")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RestaurantMeal_Restaurant"),
                    j =>
                    {
                        j.HasKey("IdRestaurant", "IdMeal");
                        j.ToTable("RestaurantMeal");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
