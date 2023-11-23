using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AfterTaste.Data;
using AfterTaste.Models;
using AfterTaste.ViewModels;

namespace AfterTaste.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(SignInViewModel loginInfo)
        {
            var result = await _signInManager.PasswordSignInAsync(loginInfo.UserName, loginInfo.Password, loginInfo.RememberMe, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "User Credentials Do Not Match");
            }
            return View(loginInfo);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpViewModel userEnteredData)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.UserName = userEnteredData.username;
                newUser.Firstname = userEnteredData.firstName;
                newUser.Lastname = userEnteredData.lastName;
                newUser.Email = userEnteredData.email;
                newUser.Address = userEnteredData.address;
                newUser.PhoneNumber = userEnteredData.contactNumber;

                var result = await _userManager.CreateAsync(newUser, userEnteredData.userPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(userEnteredData);
        }

    }
}