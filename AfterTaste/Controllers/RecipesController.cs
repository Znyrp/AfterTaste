using Microsoft.AspNetCore.Mvc;

namespace AfterTaste.Controllers
{
    public class RecipesController : Controller
    {
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
    }
}