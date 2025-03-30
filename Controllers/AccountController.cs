using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> loginManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> loginManager)
        {
            this.userManager = userManager;
            this.loginManager = loginManager;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new IdentityUser based on registration information
                var identityUser = new IdentityUser
                {
                    UserName = registerModel.Username,
                    Email = registerModel.Email
                };

                // Attempt to create the user
                var identityResult = await userManager.CreateAsync(identityUser, registerModel.Password);

                if (identityResult.Succeeded)
                {
                    // Assign the "User" role to the newly created user
                    var userRoleAsign = await userManager.AddToRoleAsync(identityUser, "User");

                    if (userRoleAsign.Succeeded)
                    {
                        // Redirect to the registration page on success
                        return RedirectToAction("Register");
                    }
                }
            }

            // If the model state is not valid, return to the registration view
            return BadRequest("Failed to register");
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                // Attempt to sign in the user with provided credentials
                var loginResult = await loginManager.PasswordSignInAsync(loginModel.Username, loginModel.Password, false, false);

                if (loginResult != null && loginResult.Succeeded)
                {
                    // Redirect to the return URL if provided, otherwise redirect to the home page
                    if (!string.IsNullOrWhiteSpace(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            // If login is not successful or the model state is not valid, return to the login view
            return BadRequest("Failed to login");
        }

        // GET: /Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user and redirect to the home page
            await loginManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
