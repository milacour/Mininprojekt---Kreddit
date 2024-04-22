using Microsoft.EntityFrameworkCore;
using shared.Model;
using web_api.Data;

namespace web_api.Services
{

    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId) =>
            await _context.Comments.Where(c => c.PostId == postId).ToListAsync();

        public async Task<Comment> GetCommentByIdAsync(int commentId) =>
            await _context.Comments.FindAsync(commentId);

        public async Task<Comment> CreateCommentAsync(Comment newComment)
        {
            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();
            return newComment;
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _context.Entry(comment).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}