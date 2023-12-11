﻿using AfterTaste.Data;
using AfterTaste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Dashboard()
        {
            return View(_dbData.Recipes);
        }

		public IActionResult Responses()
		{
			return View(_dbData.ContactUs);
		}

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
    }
}