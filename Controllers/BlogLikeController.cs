using Blog.Models.Domain;
using Blog.Models.ViewModels;
using Blog.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    // ToDo: Fix like button not working
    [Route("api/[controller]")]
    [ApiController]
    public class BlogLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository likeRepo;

        public BlogLikeController(IBlogPostLikeRepository likeRepo)
        {
            this.likeRepo = likeRepo;
        }

        // POST: api/BlogLike/Add
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest likeRequest)
        {
            // Create a new BlogPostLike model based on the request
            var model = new BlogPostLike
            {
                BlogPostId = likeRequest.BlogPostId,
                UserId = likeRequest.UserId
            };

            // Add the like to the repository
            await likeRepo.AddLike(model);

            // Return Ok status
            return Ok();
        }

        //ToDo: finish making the dislike option
        [HttpPost]
        [Route("Dislike")]
        public async Task<IActionResult> AddDislike([FromBody] AddDislikeRequest disRequest)
        {
            return null;
        }

        // ToDo: Fix Remove Like option
        // POST: api/BlogLike/Remove
        [HttpPost]
        [Route("Remove")]
        public async Task<IActionResult> RemoveLike([FromBody] AddLikeRequest likeRequest)
        {
            // Create a new BlogPostLike model based on the request
            var model = new BlogPostLike
            {
                BlogPostId = likeRequest.BlogPostId,
                UserId = likeRequest.UserId
            };

            // Remove the like from the repository
            await likeRepo.RemoveLike(model);

            // ToDo: Message if like was removed

            // Return Ok status
            return Ok();
        }

        // GET: api/BlogLike/PostId:Guid
        [HttpGet]
        [Route("PostId:Guid")]
        public async Task<IActionResult> GetTotalLikes([FromRoute] Guid postId)
        {
            // Retrieve the total amount of likes for a specific post
            var totalAmountOfLikes = await likeRepo.GetLikeTotal(postId);

            // Return Ok status with the total amount of likes
            return Ok(totalAmountOfLikes);
        }
    }
}
