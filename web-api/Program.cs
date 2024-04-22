using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using shared.Model; // Erstat med dit faktiske namespace for dine modeller
using web_api.Data;
using web_api.Services; // Antager at dine services ligger i web_api.Services namespace

var builder = WebApplication.CreateBuilder(args);

// Tilføj MVC og API Explorer services for Swagger dokumentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kreddit API", Version = "v1" });
});

// Konfigurer services og DbContext som før
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Konfigurer CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policyBuilder =>
        policyBuilder.WithOrigins("https://localhost:7228")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

// Anvend Swagger middleware kun i udviklingsmiljøet
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kreddit API v1"));
}

// Anvend CORS policy
app.UseCors("MyCorsPolicy");


// Posts endpoints
app.MapGet("/posts", async (IPostService postService) => await postService.GetAllPostsAsync());

app.MapGet("/posts/{id}", async (int id, IPostService postService) =>
{
    var post = await postService.GetPostByIdAsync(id);
    return post != null ? Results.Ok(post) : Results.NotFound();
});

app.MapPost("/posts", async (Post post, IPostService postService) =>
{
    await postService.CreatePostAsync(post);
    return Results.Created($"/posts/{post.Id}", post);
});

app.MapPut("/posts/{id}", async (int id, Post updatedPost, IPostService postService) =>
{
    var post = await postService.GetPostByIdAsync(id);
    if (post == null) return Results.NotFound();

    post.Title = updatedPost.Title;
    post.Content = updatedPost.Content;

    await postService.UpdatePostAsync(post);
    return Results.NoContent();
});

app.MapDelete("/posts/{id}", async (int id, IPostService postService) =>
{
    await postService.DeletePostAsync(id);
    return Results.NoContent();
});

// Posts up/down-votes endpoints
app.MapPut("/posts/{id}/upvote", async (int id, IPostService postService) =>
{
    var post = await postService.GetPostByIdAsync(id);
    if (post == null)
    {
        return Results.NotFound();
    }
    post.Upvotes += 1;
    await postService.UpdatePostAsync(post);
    return Results.Ok(post);
});

app.MapPut("/posts/{id}/downvote", async (int id, IPostService postService) =>
{
    var post = await postService.GetPostByIdAsync(id);
    if (post == null)
    {
        return Results.NotFound();
    }
    post.Downvotes += 1;
    await postService.UpdatePostAsync(post);
    return Results.Ok(post);
});


// Comments endpoints
app.MapGet("/posts/{postId}/comments", async (int postId, ICommentService commentService) =>
    await commentService.GetCommentsByPostIdAsync(postId));

app.MapPost("/comments", async (Comment comment, ICommentService commentService) =>
{
    await commentService.CreateCommentAsync(comment);
    return Results.Created($"/comments/{comment.Id}", comment);
});

app.MapPut("/comments/{id}", async (int id, Comment updatedComment, ICommentService commentService) =>
{
    var comment = await commentService.GetCommentByIdAsync(id);
    if (comment == null) return Results.NotFound();

    comment.Content = updatedComment.Content;

    await commentService.UpdateCommentAsync(comment);
    return Results.NoContent();
});

app.MapDelete("/comments/{id}", async (int id, ICommentService commentService) =>
{
    await commentService.DeleteCommentAsync(id);
    return Results.NoContent();
});

app.Run();
