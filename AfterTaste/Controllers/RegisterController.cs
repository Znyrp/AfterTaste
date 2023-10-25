using Microsoft.AspNetCore.Mvc;

namespace AfterTaste.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
    }
}
