using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Controllers
{
    [Authorize]
    public class UserBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogRepository;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> loginManager;

        public UserBlogPostController(ITagRepository tagRepository, IBlogPostRepository blogRepository,
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> loginManager)
        {
            this.tagRepository = tagRepository;
            this.blogRepository = blogRepository;
            this.userManager = userManager;
            this.loginManager = loginManager;
        }

        // GET: UserBlogPost/Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            // Retrieve all tags from the repository
            var tags = await tagRepository.GetAllAsync();

            // Create a view model with tag options for the user
            var model = new AddBlogPostRequestUser
            {
                Tags = tags.Select(t => new SelectListItem { Text = t.DisplayName, Value = t.Id.ToString() })
            };

            // Return the view with the model
            return View(model);
        }

        // POST: UserBlogPost/Add
        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequestUser request)
        {
            // Generate a unique identifier (you can use a GUID for simplicity)
            string urlId = Guid.NewGuid().ToString("N");

            // Combine the title and unique identifier and create a URL-friendly string
            string urlHandle = $"{request.Author}-{urlId}".ToLower().Replace(" ", "-");

            // Create a new BlogPost object with the data from the request
            var blogPost = new BlogPost
            {
                Heading = request.Heading,
                PageTitle = request.PageTitle,
                Content = request.Content,
                ShortDescription = request.ShortDescription,
                FeaturedImageUrl = request.FeaturedImageUrl,
                UrlHandle = urlHandle,
                PublishedDate = DateTime.Now,
                Author = request.Author,
                Visible = request.Visible,
            };

            // Set the user ID if the user is signed in
            if (loginManager.IsSignedIn(User))
            {
                blogPost.UserId = Guid.Parse(userManager.GetUserId(User));
            }

            // Retrieve and assign selected tags from the request
            var selectedTags = new List<Tag>();
            foreach (var tagId in request.SelectedTags)
            {
                var Id = Guid.Parse(tagId);
                var tag = await tagRepository.GetAsync(Id);

                if (tag != null)
                {
                    selectedTags.Add(tag);
                }
            }
            blogPost.Tags = selectedTags;

            // Add the new blog post to the repository
            await blogRepository.AddAsync(blogPost);

            // Set a temporary message and redirect to the home page
            TempData["Message"] = "Your post request has been saved! The site admin will review it shortly.";
            return RedirectToAction("Index", "Home");
        }
    }
}
