using shared.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace kreddit_app.Data;

public class ApiService
{
    private readonly HttpClient http;
    private readonly string baseAPI;

    public ApiService(HttpClient http, IConfiguration configuration)
    {
        this.http = http;
        // Her justeres navnet for at matche nøglen i din konfigurationsfil
        this.baseAPI = configuration["ApiSettings:BaseApiUrl"];
    }

    public async Task<Post[]> GetPosts()
    {
        string url = $"{baseAPI}posts/";
        return await http.GetFromJsonAsync<Post[]>(url);
    }

    public async Task<Post> GetPost(int id)
    {
        string url = $"{baseAPI}posts/{id}/";
        return await http.GetFromJsonAsync<Post>(url);
    }

    public async Task<Comment> CreateComment(string content, int postId, int userId)
    {
        string url = $"{baseAPI}posts/{postId}/comments";

        HttpResponseMessage msg = await http.PostAsJsonAsync(url, new { content, userId });

        // Anvend 'await' i stedet for '.Result' for at undgå potentielle deadlocks
        string json = await msg.Content.ReadAsStringAsync();

        Comment? newComment = JsonSerializer.Deserialize<Comment>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return newComment;
    }

    public async Task<Post> UpvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/upvote/";

        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Anvend 'await' i stedet for '.Result' for at undgå potentielle deadlocks
        string json = await msg.Content.ReadAsStringAsync();

        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return updatedPost;
    }

    public async Task<Post> DownvotePost(int id)
    {
        string url = $"{baseAPI}posts/{id}/downvote/";

        HttpResponseMessage msg = await http.PutAsJsonAsync(url, "");

        // Anvend 'await' til at læse responsindholdet asynkront.
        string json = await msg.Content.ReadAsStringAsync();

        // Deserialiser JSON-strengen til en Post-instans. Sikr, at ejendomsnavne behandles uden forskel på store og små bogstaver.
        Post? updatedPost = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Returner den opdaterede post.
        return updatedPost;
    }

}
