using AfterTaste.Data;
using AfterTaste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AfterTaste.Controllers
{
    public class RecipesController : Controller
    {
        private readonly AppDbContext _dbData;
        public RecipesController(AppDbContext dbData)
        {
            _dbData = dbData;
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

        public IActionResult RecipeDetails(int id)
        {
            //Search for the recipe whose id matches the given id
            Recipe? recipe = _dbData.Recipes.FirstOrDefault(ins => ins.recipeId == id);

            if (recipe != null)//was a recipe found?
                return View(recipe);

            return NotFound();
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddRecipe()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRecipe(Recipe newRecipe)
        {
            if(!ModelState.IsValid)
                return View();

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
        public IActionResult UpdateRecipe(Recipe recipeChanges)
        {
            Recipe? recipe = _dbData.Recipes.FirstOrDefault(rec => rec.recipeId == recipeChanges.recipeId);

            if (recipe != null)
            {
                recipe.recipeName = recipeChanges.recipeName;
                recipe.recipeDescription = recipeChanges.recipeDescription;
                recipe.recipeVideo = recipeChanges.recipeVideo;
                recipe.Origin = recipeChanges.Origin;
                recipe.recipeDirections = recipeChanges.recipeDirections;
                recipe.recipeIngredients = recipeChanges.recipeIngredients;

                _dbData.SaveChanges(); // Save changes after making all the necessary updates
            }

            return RedirectToAction("TopRatedRecipe");
        }
    }
}