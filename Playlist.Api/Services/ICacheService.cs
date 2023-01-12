namespace Playlist.Api.Services
{
    public interface ICacheService<T> where T : class
    {
        void SetItems(List<T> items, string username);
        List<T> GetItems(string username);
        void RemoveItems(string username);
    }
}
