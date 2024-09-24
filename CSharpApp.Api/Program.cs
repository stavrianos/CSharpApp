var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddDefaultConfiguration();

builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["BaseUrl"]!);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/todos", async (ITodoService todoService) =>
    {
        try
        {
            var todos = await todoService.GetAllTodos();
            return Results.Ok(todos);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching todos.");
            return Results.Problem("There was an error fetching todos.");
        }
    })
    .WithName("GetTodos")
    .WithOpenApi();

app.MapGet("/todos/{id:int}", async ([FromRoute] int id, ITodoService todoService) =>
    {
        try
        {
            var todo = await todoService.GetTodoById(id);
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error fetching todo with id {id}.");
            return Results.Problem("There was an error fetching the todo.");
        }
    })
    .WithName("GetTodosById")
    .WithOpenApi();

app.MapGet("/posts", async (IPostService postService) =>
    {
        try
        {
            var posts = await postService.GetAllPosts();
            return Results.Ok(posts);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error fetching posts.");
            return Results.Problem("There was an error fetching posts.");
        }
    })
    .WithName("GetPosts")
    .WithOpenApi();

app.MapGet("/posts/{id:int}", async ([FromRoute] int id, IPostService postService) =>
    {
        try
        {
            var post = await postService.GetPostById(id);
            return post is not null ? Results.Ok(post) : Results.NotFound();
        }
        catch (Exception ex)
        {
            Log.Error(ex, $"Error fetching post with id {id}.");
            return Results.Problem("There was an error fetching the post.");
        }
    })
    .WithName("GetPostsById")
    .WithOpenApi();

app.MapPost("/posts",
        async ([FromBody] PostRecord postRecord, IPostService postService) =>
        {
            try
            {
                await postService.CreatePost(postRecord);
                return Results.Created($"/posts/{postRecord.Id}", postRecord);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating post.");
                return Results.Problem("There was an error creating the post.");
            }
        })
    .WithName("CreatePosts")
    .WithOpenApi();

app.MapDelete("/posts/{id:int}",
        async ([FromRoute] int id, IPostService postService) =>
        {
            try
            {
                await postService.DeletePostById(id);
                return Results.NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error deleting post with id {id}.");
                return Results.Problem("There was an error deleting the post.");
            }
        })
    .WithName("DeletePostsById")
    .WithOpenApi();

app.Run();