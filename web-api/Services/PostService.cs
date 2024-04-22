using Microsoft.EntityFrameworkCore;
using shared.Model;
using web_api.Data;

namespace web_api.Services
{

    public class PostService : IPostService
    {
        private readonly AppDbContext _context;

        public PostService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync() =>
            await _context.Posts.ToListAsync();

        public async Task<Post> GetPostByIdAsync(int postId) =>
            await _context.Posts.FindAsync(postId);

        public async Task<Post> CreatePostAsync(Post newPost)
        {
            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            return newPost;
        }

        public async Task UpdatePostAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
        }

    }
}