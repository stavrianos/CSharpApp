namespace CSharpApp.Core.Interfaces;

public interface IPostService
{
    Task<PostRecord?> GetPostById(int id);
    Task<ReadOnlyCollection<PostRecord>> GetAllPosts();
    Task CreatePost(PostRecord record);
    Task DeletePostById(int id);
}