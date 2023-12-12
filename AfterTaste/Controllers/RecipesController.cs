using AfterTaste.Data;
using AfterTaste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AfterTaste.Controllers
{
    public class RecipesController : Controller
    {
        private readonly AppDbContext _dbData;
        private readonly UserManager<User> _userManager;
        public RecipesController(AppDbContext dbData, UserManager<User> userManager)
        {
            _dbData = dbData;
            _userManager = userManager;
        }
        public IActionResult Adobo()
        {
            return View();
        }
        public IActionResult KoreanFriedChicken()
        {
            return View();
        }
        public IActionResult IndianStew()
        {
            return View();
        }
        public IActionResult ItalianMeatballs()
        {
            return View();
        }

        public IActionResult TopRatedRecipe()
        {
            return View(_dbData.Recipes);
        }

        public IActionResult ExploreRecipes()
        {
            return View(_dbData.Recipes);
        }

        public async Task<IActionResult> RecipeDetails(int id)
        {
            //Search for the recipe whose id matches the given id
            Recipe? recipe = await _dbData.Recipes
                .Include(r => r.User) // Include the User navigation property in the Recipe
                .FirstOrDefaultAsync(ins => ins.recipeId == id);

            if (recipe != null)
            {
                var reviews = await _dbData.UserReviews
                    .Include(r => r.User) // Include the User navigation property in UserReview
                    .Where(r => r.RecipeId == id)
                    .ToListAsync();

                var userIds = reviews.Select(r => r.userId).Distinct().ToList();

                // Fetch user details for all unique user IDs from reviews
                var users = await _dbData.Users
                    .Where(u => userIds.Contains(u.Id))
                    .ToListAsync();

                ViewBag.Reviews = reviews;
                ViewBag.Users = users;

                return View(recipe);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddRecipe()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe(Recipe newRecipe, IFormFile? recipeImage)
        {
            if (!ModelState.IsValid)
                return View();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (recipeImage != null && recipeImage.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await recipeImage.CopyToAsync(memoryStream);
                newRecipe.recipeImage = memoryStream.ToArray();
            }

            newRecipe.Status = RecipeStatus.Pending;

            // Set the UserId of the new recipe
            newRecipe.userId = userId;

            //Convert youtube link to youtube embeddable link
            newRecipe.recipeVideo = ConvertToEmbedUrl(newRecipe.recipeVideo);


            _dbData.Recipes.Add(newRecipe);
            _dbData.SaveChanges();
            return RedirectToAction("TopRatedRecipe");
        }

        [HttpGet]
        public IActionResult UpdateRecipe(int id)
        {
            //Search for recipe whose id matches the given id
            Recipe? recipe = _dbData.Recipes.FirstOrDefault(rec => rec.recipeId == id);

            if (recipe != null)//was a recipe found?
                return View(recipe);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecipe(Recipe recipeChanges, IFormFile? changeImage)
        {
            if (ModelState.IsValid)
            {
                Recipe? recipe = _dbData.Recipes.FirstOrDefault(rec => rec.recipeId == recipeChanges.recipeId);

                if (recipe != null)
                {
                    recipe.recipeName = recipeChanges.recipeName;
                    recipe.recipeDescription = recipeChanges.recipeDescription;
                    recipe.Origin = recipeChanges.Origin;
                    recipe.recipeVideo = recipeChanges.recipeVideo;
                    recipe.recipeIngredients = recipeChanges.recipeIngredients;
                    recipe.recipeDirections = recipeChanges.recipeDirections;
                    recipe.recipeCalories = recipeChanges.recipeCalories;

                    if (changeImage != null && changeImage.Length > 0)
                    {
                        using MemoryStream memoryStream = new MemoryStream();
                        await changeImage.CopyToAsync(memoryStream);
                        recipe.recipeImage = memoryStream.ToArray();
                    }
                    else
                    {
                        recipe.recipeImage = null; // Set recipeImage to null if no image provided
                    }

                    _dbData.Entry(recipe).State = EntityState.Modified;
                    await _dbData.SaveChangesAsync();

                    return RedirectToAction("TopRatedRecipe");
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return View("UpdateRecipe", recipeChanges);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            Recipe? recipe = await _dbData.Recipes.FirstOrDefaultAsync(rec => rec.recipeId == id);

            if (recipe != null)
            {
                return View(recipe);
            }

            return NotFound();
        }

        [HttpPost]
        [ActionName("DeleteRecipe")]
        public async Task<IActionResult> DeleteRecipeConfirmed(int id)
        {
            Recipe? recipe = await _dbData.Recipes.FindAsync(id);

            if (recipe != null)
            {
                _dbData.Recipes.Remove(recipe);
                await _dbData.SaveChangesAsync();
                return RedirectToAction("TopRatedRecipe");
            }

            return NotFound();
        }

        public static string ConvertToEmbedUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            // Extract the video ID from the URL
            var videoId = new Uri(url).GetComponents(UriComponents.Query, UriFormat.Unescaped).Split('=')[1];

            // Convert the video URL to an embed URL
            var video = $"https://www.youtube.com/embed/{videoId}";
            return video;
        }

        [Authorize]
        public async Task<IActionResult> AddFavorite(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var recipe = await _dbData.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            var existingFavorite = await _dbData.Favorites.FirstOrDefaultAsync(f => f.recipeId == id && f.userId == userId);

            if (existingFavorite != null)
            {
                // Recipe is unfavorited
                _dbData.Favorites.Remove(existingFavorite);
                await _dbData.SaveChangesAsync();
                TempData[$"Message_{id}"] = "Recipe unfavorited successfully";
            }
            else
            {
                // Recipe is favorited
                var favorite = new FavoriteRecipe
                {
                    userId = userId,
                    recipeId = recipe.recipeId
                };

                _dbData.Favorites.Add(favorite);
                await _dbData.SaveChangesAsync();
                TempData[$"Message_{id}"] = "Recipe favorited successfully";
            }

            // Determine the referrer URL
            string referrer = Request.Headers["Referer"].ToString();

            // Check if the referrer is the RecipeDetails URL
            if (referrer.Contains("RecipeDetails"))
            {
                return RedirectToAction("RecipeDetails", new { id = id });
            }
            else
            {
                return RedirectToAction("TopRatedRecipe");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddReview(int recipeId)
        {
            // Validate the recipeId here before proceeding
            var reviewModel = new UserReview { RecipeId = recipeId };
            return View(reviewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddReview(UserReview reviewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(reviewModel);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user has already reviewed this recipe
            var existingReview = await _dbData.UserReviews
                .FirstOrDefaultAsync(r => r.RecipeId == reviewModel.RecipeId && r.userId == userId);

            if (existingReview != null)
            {
                // User has already reviewed this recipe, handle accordingly (e.g., show error message)
                ModelState.AddModelError("", "You have already reviewed this recipe.");
                return View(reviewModel);
            }

            try
            {
                var userReview = new UserReview
                {
                    userId = userId,
                    Rating = reviewModel.Rating,
                    ReviewDate = DateTime.Now,
                    RecipeId = reviewModel.RecipeId,
                    comment = reviewModel.comment
                };

                _dbData.UserReviews.Add(userReview);
                await _dbData.SaveChangesAsync();

                return RedirectToAction("RecipeDetails", new { id = reviewModel.RecipeId });
            }
            catch (Exception ex)
            {
                // Log the exception or provide an error message
                return RedirectToAction("RecipeDetails", new { id = reviewModel.RecipeId });
            }
        }




    }
}