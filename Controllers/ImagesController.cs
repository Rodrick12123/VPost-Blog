using Blog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IimageRepository imageRepository;

        public ImagesController(IimageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        // POST: api/Images
        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            // Upload the image and get the image URL
            var imageUrl = await imageRepository.UploadAsync(file);

            // Check if the upload was successful
            if (imageUrl == null)
            {
                // Return a problem response with a 500 Internal Server Error status
                return Problem("Something went wrong!", null, (int)HttpStatusCode.InternalServerError);
            }

            // Return a JSON response with the image URL
            return new JsonResult(new { link = imageUrl });
        }
    }
}
