using Microsoft.AspNetCore.Mvc;

namespace AfterTaste.Controllers
{
    public class RecipesController : Controller
    {
        public IActionResult Adobo()
        {
            return View();
        }
    }
}
