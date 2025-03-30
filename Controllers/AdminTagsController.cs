using Blog.Data;
using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        // GET: /AdminTags/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /AdminTags/Add
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest request)
        {
            // Check if the model is valid
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Create a new tag using form data
            var tag = new Tag
            {
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            // Include a check if the Name already exists

            // Add the tag to the repository
            await tagRepository.AddAsync(tag);

            // Redirect to the list page after successful addition
            return RedirectToAction("List");
        }

        // GET: /AdminTags/List
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // Use the dbContext to read the tags
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        // GET: /AdminTags/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            // Fetch the tag for editing
            var tag = await tagRepository.GetAsync(id);
            if (tag != null)
            {
                // Map the tag to an EditTagRequest model
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }

            // Return null if the tag does not exist
            return View(null);
        }

        // POST: /AdminTags/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest request)
        {
            // Map the EditTagRequest model to a Tag domain model
            var tag = new Tag
            {
                Id = request.Id,
                Name = request.Name,
                DisplayName = request.DisplayName
            };

            // Update the tag in the repository
            var existingTag = await tagRepository.UpdateAsync(tag);

            if (existingTag != null)
            {
                // Handle success
            }
            else
            {
                // Handle failure
            }

            // Redirect to the edit page after update
            return RedirectToAction("Edit", new { id = request.Id });
        }

        // POST: /AdminTags/Delete
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest request)
        {
            // Delete the tag and redirect to the list page
            var tag = await tagRepository.DeleteAsync(request.Id);

            if (tag != null)
            {
                // Handle success
                return RedirectToAction("List");
            }

            // Redirect to the edit page if deletion fails
            return RedirectToAction("Edit", new { id = request.Id });
        }
    }
}
