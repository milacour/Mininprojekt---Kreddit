using shared.Model;

namespace web_api.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int postId);
        Task<Post> CreatePostAsync(Post newPost);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int postId);
    }
}