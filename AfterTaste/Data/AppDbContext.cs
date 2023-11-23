using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AfterTaste.Models;

namespace AfterTaste.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Recipe> Recipes { get; set; }
    

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    //Data Seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // do not remove this!

        modelBuilder.Entity<Recipe>().HasData(
            new Recipe()
            {
                recipeId = 1,
                recipeName = "Adobong Manok",
                recipeDescription = "Chicken Adobo is an authentic Filipino dish and is one of the mostly recognized Filipino foods.",
                recipeVideo = "https://www.youtube.com/embed/XtFu4wUN2Ok?start=1",
                recipeImage = "Test",
                recipeDirections = "Heat oil in cooking pot, Saute the garlic",
                recipeIngredients = "Oil - 3 Tablespoons, Garlic - 1 whole choppoed",
                Origin = Origin.SouthKorea
            },
            new Recipe()
            {
                recipeId = 2,
                recipeName = "Hello Manok",
                recipeDescription = "Hello Manok Adobo is an authentic Filipino dish and is one of the mostly recognized Filipino foods.",
                recipeVideo = "https://www.youtube.com/embed/FIzEU7K8H5c?start=1",
                recipeImage = "Test",
                recipeDirections = "Heat oil in cooking pot, Saute the garlic",
                recipeIngredients = "Oil - 3 Tablespoons, Garlic - 1 whole choppoed",
                Origin = Origin.India
            }

            );
    }
}