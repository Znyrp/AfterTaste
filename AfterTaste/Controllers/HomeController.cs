using AfterTaste.Data;
using AfterTaste.Models;
using AfterTaste.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AfterTaste.Controllers
{
	public class HomeController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly AppDbContext _dbContext;
		private readonly ILogger<HomeController> _logger;

		public HomeController(UserManager<User> userManager, AppDbContext dbContext, ILogger<HomeController> logger)
		{
			_userManager = userManager;
			_dbContext = dbContext;
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

        public IActionResult Profile()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
            {
                // Handle user not found
                return RedirectToAction("Index"); // Redirect to another action or handle appropriately
            }

            // Fetch the user's created recipes
            var createdRecipes = _dbContext.Recipes.Where(r => r.userId == user.Id).ToList();

            // Fetch the user's favorite recipe IDs
            var favoriteRecipeIds = _dbContext.Favorites.Where(f => f.userId == user.Id).Select(f => f.recipeId).ToList();

			// Fetch the user's favorite recipes based on the IDs
			var favoriteRecipes = _dbContext.Recipes.Where(r => favoriteRecipeIds.Contains(r.recipeId)).ToList();

            var viewModel = new ProfileViewModel
            {
                CreatedRecipes = createdRecipes,
                FavoriteRecipes = favoriteRecipes
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
