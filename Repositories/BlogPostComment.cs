using Blog.Data;
using Blog.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repositories
{
    public class BlogPostComment : IBlogPostCommentRepository
    {
        private readonly BlogDbContext blogContext;

        public BlogPostComment(BlogDbContext blogContext)
        {
            this.blogContext = blogContext;
        }

        // Adds a new blog comment to the database
        public async Task<PostComment> AddAsync(PostComment blogComment)
        {
            await blogContext.PostComments.AddAsync(blogComment);
            await blogContext.SaveChangesAsync();
            return blogComment;
        }

        // Retrieves all comments associated with a specific blog post
        public async Task<IEnumerable<PostComment>> GetAllCommentsByPostId(Guid postId)
        {
            var comments = await blogContext.PostComments
                .Where(p => p.BlogPostId == postId)
                .ToListAsync();
            return comments;
        }
    }
}
