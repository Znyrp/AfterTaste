using AfterTaste.Data;
using AfterTaste.Models;
using AfterTaste.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

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

            var createdRecipes = _dbContext.Recipes.Where(r => r.userId == user.Id).ToList();

            var favoriteRecipeIds = _dbContext.Favorites.Where(f => f.userId == user.Id).Select(f => f.recipeId).ToList();

            var favoriteRecipes = _dbContext.Recipes.Include(r => r.User).Where(r => favoriteRecipeIds.Contains(r.recipeId))
                .ToList();

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

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(int recipeId, int rating, string userComment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var existingReview = await _dbContext.UserReviews.FirstOrDefaultAsync(r => r.RecipeId == recipeId && r.userId == userId);

                if (existingReview != null)
                {
                    ModelState.AddModelError("", "You have already reviewed this recipe.");
                    return RedirectToAction("RecipeDetails", new { id = recipeId });
                }

                var userReview = new UserReview
                {
                    userId = userId,
                    Rating = rating,
                    ReviewDate = DateTime.Now,
                    RecipeId = recipeId,
                    comment = userComment
                };

                _dbContext.UserReviews.Add(userReview);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("RecipeDetails", new { id = recipeId });
            }
            catch (Exception ex)
            {
                // Log the exception or provide an error message
                ModelState.AddModelError("", "An error occurred while adding the review.");
                return RedirectToAction("RecipeDetails", new { id = recipeId });
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddContact(ContactUs newContact)
        {
            if (!ModelState.IsValid)
                return View();

            var currentUser = _userManager.GetUserAsync(User).Result;
            if (currentUser == null)
            {
                // Handle user not found
                return RedirectToAction("Index");
            }

            // Prefill the fullName and email fields with the currently logged-in user's info
            newContact.fullName = $"{currentUser.Firstname} {currentUser.Lastname}";
            newContact.email = currentUser.Email;

            _dbContext.ContactUs.Add(newContact);
            _dbContext.SaveChanges();

            // Redirect to the index page or wherever appropriate
            return RedirectToAction("Index");
        }



    }
}
