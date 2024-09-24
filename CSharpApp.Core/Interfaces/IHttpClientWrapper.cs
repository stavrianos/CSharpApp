namespace CSharpApp.Core.Interfaces;

public interface IHttpClientWrapper
{
    Task<TResponse?> GetAsync<TResponse>(string url);
    Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);
    Task<TResponse?> PutAsync<TRequest, TResponse>(string url, TRequest data);
    Task DeleteAsync<TResponse>(string url);
}