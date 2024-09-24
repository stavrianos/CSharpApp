namespace CSharpApp.Application.Services;

public class PostService : IPostService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly ILogger<PostService> _logger;

    public PostService(IHttpClientWrapper httpClientWrapper, ILogger<PostService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientWrapper);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClientWrapper = httpClientWrapper;
        _logger = logger;
    }

    public async Task<PostRecord?> GetPostById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(id, 1, nameof(id));

            var response = await _httpClientWrapper.GetAsync<PostRecord>($"posts/{id}");
            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Getting post by id {id} threw an exception {Exception} with message {Message}", id, e,
                e.Message);
            throw;
        }
    }

    public async Task<ReadOnlyCollection<PostRecord>> GetAllPosts()
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync<List<PostRecord>>("posts");

            return response?.AsReadOnly() ?? ReadOnlyCollection<PostRecord>.Empty;
        }
        catch (Exception e)
        {
            _logger.LogError("Getting all posts threw an exception {Exception} with message {Message}", e, e.Message);
            throw;
        }
    }

    public async Task CreatePost(PostRecord record)
    {
        try
        {
            await _httpClientWrapper.PostAsync<PostRecord, HttpResponseMessage>("posts", record);
        }
        catch (Exception e)
        {
            _logger.LogError("Creating new post {Record} threw an exception {Exception} with message {Message}", record,
                e,
                e.Message);
            throw;
        }
    }

    public async Task DeletePostById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(id, 1, nameof(id));
            await _httpClientWrapper.DeleteAsync<HttpResponseMessage>($"posts/{id}");
        }
        catch (Exception e)
        {
            _logger.LogError("Deleting a post by id {id} threw an exception {Exception} with message {Message}", id, e,
                e.Message);
            throw;
        }
    }
}