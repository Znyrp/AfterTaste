using AfterTaste.Data;
using AfterTaste.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AfterTaste.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbData;
        private readonly UserManager<User> _userManager;
        public AdminController(AppDbContext dbData, UserManager<User> userManager)
        {
            _dbData = dbData;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            return View(_dbData.Recipes);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Responses()
		{
			return View(_dbData.ContactUs);
		}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int recipeId, RecipeStatus status)
        {
            var recipe = _dbData.Recipes.FirstOrDefault(r => r.recipeId == recipeId);
            if (recipe != null)
            {
                recipe.Status = status;
                await _dbData.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard"); // Or any other appropriate action
        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ActionName("DeleteRecipe")]
        public async Task<IActionResult> DeleteRecipeConfirmed(int id)
        {
            Recipe? recipe = await _dbData.Recipes.FindAsync(id);

            if (recipe != null)
            {
                _dbData.Recipes.Remove(recipe);
                await _dbData.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }

            return NotFound();
        }
    }
}
