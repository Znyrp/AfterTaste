using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AfterTaste.Models;

namespace AfterTaste.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<FavoriteRecipe> Favorites { get; set; }
    

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    //Data Seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // do not remove this!

        modelBuilder.Entity<Recipe>();

        modelBuilder.Entity<FavoriteRecipe>();
     
    }
}