using Microsoft.Extensions.Caching.Memory;

namespace Playlist.Api.Services
{
    public class CacheService<T>: ICacheService<T> where T : class
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Cached the list of items
        public void SetItems(List<T> items, string username)
        {
            if (!items.Any())
                throw new ArgumentNullException(nameof(items));

            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var key = $"{username}-{typeof(T)}s";

            _cache.Set(key, items, TimeSpan.FromMinutes(5));
        }

        // Retrieve the group of cached items
        public List<T> GetItems(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var key = $"{username}-{typeof(T)}s";

            return _cache.Get<List<T>>(key);
        }

        // Delete the group of cached items
        public void RemoveItems(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var key = $"{username}-{typeof(T)}s";

            _cache.Remove(key);
        }
    }
}