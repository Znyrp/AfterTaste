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
                var user = await _userManager.FindByNameAsync(loginInfo.UserName);

                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction("Dashboard", "Admin"); // Redirect admin to the dashboard
                }

                return RedirectToAction("Index", "Home"); // Redirect regular users to the homepage
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
        public async Task<IActionResult> Register(SignUpViewModel userEnteredData, IFormFile? profilePicture)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.UserName = userEnteredData.username;
                newUser.Firstname = userEnteredData.firstName;
                newUser.Lastname = userEnteredData.lastName;
                newUser.Email = userEnteredData.email;
                newUser.Address = userEnteredData.address;
                newUser.Birthdate = userEnteredData.birthdate;
                newUser.PhoneNumber = userEnteredData.contactNumber;

                if (profilePicture != null && profilePicture.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await profilePicture.CopyToAsync(memoryStream);
                    newUser.ProfilePicture = memoryStream.ToArray();
                }
                else
                {
                    newUser.ProfilePicture = null; // Set recipeImage to null if no image provided
                }

                var result = await _userManager.CreateAsync(newUser, userEnteredData.userPassword);
                if (result.Succeeded)
                {
					TempData["SuccessMessage"] = "Registration successful!, You may now Login";
					return RedirectToAction("Register", "Account");
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

        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                var profileData = new SignUpViewModel
                {
                    username = user.UserName,
                    firstName = user.Firstname,
                    lastName = user.Lastname,
                    email = user.Email,
                    birthdate = user.Birthdate,
                    address = user.Address,
                    contactNumber = user.PhoneNumber,
                    profilePicture = user.ProfilePicture,
                    userPassword = user.PasswordHash
                    
                };

                return View(profileData);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(SignUpViewModel updatedProfile, IFormFile? profilePicture)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                //Update Password
                if (!string.IsNullOrEmpty(updatedProfile.userPassword))
                {
                    var passwordValidator = _userManager.PasswordValidators.FirstOrDefault();
                    var passwordValidationResult = await passwordValidator.ValidateAsync(_userManager, user, updatedProfile.userPassword);

                    if (!passwordValidationResult.Succeeded)
                    {
                        foreach (var error in passwordValidationResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(updatedProfile); // Return to the view if password validation fails
                    }

                    // If validation succeeded, update the password hash
                    var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, updatedProfile.userPassword);
                    user.PasswordHash = newPasswordHash;
                }

                //Update User Profile
                if (user != null)
                {
                    user.UserName = updatedProfile.username;
                    user.Firstname = updatedProfile.firstName;
                    user.Lastname = updatedProfile.lastName;
                    user.Email = updatedProfile.email;
                    user.Address = updatedProfile.address;
                    user.Birthdate = updatedProfile.birthdate;
                    user.PhoneNumber = updatedProfile.contactNumber;

                    if (profilePicture != null && profilePicture.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await profilePicture.CopyToAsync(memoryStream);
                        user.ProfilePicture = memoryStream.ToArray();
                    }
                    else
                    {
						user.ProfilePicture = null;
					}


                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Account Updated Successfully!";

                        return RedirectToAction("EditProfile", "Account"); // Redirect to the Sign In page after sign-out
					}
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            // If ModelState is not valid, return to the view with the current data
            return View(updatedProfile);
        }


    }
}