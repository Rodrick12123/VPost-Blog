using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> loginManager;

        public AdminUserController(IUserRepository userRepository, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> loginManager)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.loginManager = loginManager;
        }

        //ToDo: Check to see if user has superadmin role
        //If so use the GetAllUsersNoSuperAdmin instead
        // GET: /AdminUser/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            // Retrieve all users from the repository
            var users = await userRepository.GetAllUsers();
            if (User.IsInRole("SuperAdmin"))
            {
                users = await userRepository.GetAllUsersWithSuperAdmin();

            }
            else
            {
                users = await userRepository.GetAllUsers();
            }

            // Create a ViewModel to display user information
            var viewUsersModel = new ViewUsers();
            viewUsersModel.Users = new List<User>();

            // Map domain model to ViewModel
            foreach (var user in users)
            {
                viewUsersModel.Users.Add(new Models.ViewModels.User
                {
                    Id = Guid.Parse(user.Id),
                    UserName = user.UserName,
                    Email = user.Email
                });
            }

            // Display the list of users
            return View(viewUsersModel);
        }

        // POST: /AdminUser/List
        [HttpPost]
        public async Task<IActionResult> List(ViewUsers request)
        {
            // Create a new IdentityUser based on the form data
            var identityUser = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            // Create the user using UserManager
            var result = await userManager.CreateAsync(identityUser, request.Password);

            if (result != null)
            {
                if (result.Succeeded)
                {
                    var roles = new List<string> { "User" };

                    // Check if the AdminCheckbox is selected to assign the Admin role
                    if (request.AdminCheckbox)
                    {
                        roles.Add("Admin");
                    }

                    // Assign roles to the user
                    result = await userManager.AddToRolesAsync(identityUser, roles);

                    if (result != null && result.Succeeded)
                    {
                        // Redirect to the user list page after successful creation
                        return RedirectToAction("List", "AdminUser");
                    }
                }
            }

            // Display the view if user creation fails
            return View();
        }

        // POST: /AdminUser/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Find the user by Id using UserManager
            var user = await userManager.FindByIdAsync(id.ToString());

            if (user != null)
            {
                // Delete the user
                var deleteUser = await userManager.DeleteAsync(user);

                if (deleteUser != null && deleteUser.Succeeded)
                {
                    // Redirect to the user list page after successful deletion
                    return RedirectToAction("List");
                }
            }

            // Display the view if user deletion fails
            return View();
        }

        //ToDo: Create an edit page that allows changing passwords and user artifacts
        //Test if id is being captured as parameter
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await userManager.FindByIdAsync(Id.ToString());
            //create view model for user model info access
            if(user != null)
            {
                var userModel = new User
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    UserName = user.UserName,
                    
                };
                return View(userModel);
            }
            

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User userRequest)
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
            var result = await userManager.UpdateAsync(identityUser);

            if (result.Succeeded)
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


            return View();
        }
    }
}
