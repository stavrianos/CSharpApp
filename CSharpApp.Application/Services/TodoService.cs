namespace CSharpApp.Application.Services;

public class TodoService : ITodoService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly ILogger<TodoService> _logger;

    public TodoService(IHttpClientWrapper httpClientWrapper, ILogger<TodoService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClientWrapper);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClientWrapper = httpClientWrapper;
        _logger = logger;
    }

    public async Task<TodoRecord?> GetTodoById(int id)
    {
        try
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(id, 1, nameof(id));

            var response = await _httpClientWrapper.GetAsync<TodoRecord>($"todos/{id}");

            return response;
        }
        catch (Exception e)
        {
            _logger.LogError("Getting todos by id {id} threw an exception {Exception} with message {Message}", id, e,
                e.Message);
            throw;
        }
    }

    public async Task<ReadOnlyCollection<TodoRecord>> GetAllTodos()
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync<List<TodoRecord>>("todos");

            return response?.AsReadOnly() ?? ReadOnlyCollection<TodoRecord>.Empty;
        }
        catch (Exception e)
        {
            _logger.LogError("Getting all todos threw an exception {Exception} with message {Message}", e, e.Message);
            throw;
        }
    }
}