namespace CSharpApp.Infrastructure.HttpClients;

public class HttpClientWrapper(HttpClient httpClient) : IHttpClientWrapper
{
    public async Task<TResponse?> GetAsync<TResponse>(string url)
    {
        using var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        using var response = await httpClient.PostAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        using var response = await httpClient.PutAsJsonAsync(url, data);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task DeleteAsync<TResponse>(string url)
    {
        using var response = await httpClient.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
}