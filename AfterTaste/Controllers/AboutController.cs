using Microsoft.AspNetCore.Mvc;

namespace AfterTaste.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
