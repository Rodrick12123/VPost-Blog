using Blog.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class ProfileManageController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> loginManager;

        public ProfileManageController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> loginManager)
        {
            this.userManager = userManager;
            this.loginManager = loginManager;
        }

        //ToDo: Create a profile management controller and page
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (loginManager.IsSignedIn(User))
            {
                var user = await userManager.FindByNameAsync(User?.Identity?.Name);
                if (user != null)
                {
                    var userModel = new User
                    {
                        Id = Guid.Parse(user.Id),
                        Email = user.Email,
                        UserName = user.UserName,

                    };
                    return View(userModel);
                }
            }
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(User userRequest)
        {
            var identityUser = await userManager.FindByNameAsync(User?.Identity?.Name);

            // Check if the identity user exists
            if (identityUser == null)
            {
                return NotFound("User not found");
            }

            // Update user's email and password
            identityUser.Email = userRequest.Email;
            identityUser.UserName = userRequest.UserName;

            // Check if a new password is provided
            if (!string.IsNullOrEmpty(userRequest.Password))
            {
                // Remove the existing password (if any)
                var removePasswordResult = await userManager.RemovePasswordAsync(identityUser);

                if (!removePasswordResult.Succeeded)
                {
                    // Handle password removal failure (e.g., return error messages)
                    return BadRequest("Failed to remove the existing password");
                }

                
                identityUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(identityUser, userRequest.Password);

                
            }

            // Update the identity user in the database
            var updateResult = await userManager.UpdateAsync(identityUser);

            if (updateResult.Succeeded)
            {
                // Success - handle accordingly (e.g., return success message)
                await loginManager.SignOutAsync();

                return Ok("User credentials updated successfully");
            }
            else
            {
                // Failed to update - handle accordingly (e.g., return error messages)
                return BadRequest("Failed to update user credentials");
            }
        }
    }
}
