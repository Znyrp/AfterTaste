using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AfterTaste.Models;

namespace AfterTaste.Data;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<FavoriteRecipe> Favorites { get; set; }
	public DbSet<ContactUs> ContactUs { get; set; }

	public DbSet<UserReview> UserReviews { get; set; }


	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    //Data Seeding
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // do not remove this!

        modelBuilder.Entity<Recipe>();

        modelBuilder.Entity<FavoriteRecipe>();

        modelBuilder.Entity<UserReview>();

        modelBuilder.Entity<ContactUs>().HasData(
			new ContactUs()
			{
				contactId = 1,
				userId = 2,
				fullName = "Alyssa",
				email = "Romen@yahoo.com",
				subject = "fgdsf",
				message = "fgdgewdfsrgthjkjlkhgyfhtdgresdfghjhgfesdfghjk,jhgfdsafghjkjhgtfrghgjk,jhgfdfg",
			});
	}
}