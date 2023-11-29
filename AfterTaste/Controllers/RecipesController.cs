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

        public async Task<IActionResult> RecipeDetails(int id)
        {
            //Search for the recipe whose id matches the given id
            Recipe? recipe = _dbData.Recipes.Include(r => r.User).FirstOrDefault(ins => ins.recipeId == id);

            if (recipe != null)
            {
                // Retrieve the user associated with the userId in the recipe
                var user = await _userManager.FindByIdAsync(recipe.userId);

                if (user != null)
                {
                    // Assign the user's first and last name to the recipe model
                    recipe.User = user;
                }

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
        public async Task<IActionResult> AddRecipe(Recipe newRecipe, IFormFile recipeImage)
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


            // Set the UserId of the new recipe
            newRecipe.userId = userId;

            _dbData.Recipes.Add(newRecipe);
            _dbData.SaveChanges();
            return RedirectToAction("TopRatedRecipe");
        }

        [HttpGet]
        public IActionResult UpdateRecipe(int id)
        {
            //Search for the instructor whose id matches the given id
            Recipe? recipe = _dbData.Recipes.FirstOrDefault(rec => rec.recipeId == id);

            if (recipe != null)//was an instructor found?
                return View(recipe);

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRecipe(Recipe recipeChanges, IFormFile changeImage)
        {
            if (ModelState.IsValid)
            {
                // Assuming 'db' is your DbContext instance
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

    }
}