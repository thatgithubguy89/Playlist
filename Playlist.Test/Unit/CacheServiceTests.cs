using Microsoft.Extensions.Caching.Memory;

namespace Playlist.Test.Unit
{
    public class CacheServiceTests
    {
        ICacheService<Video> _cacheService;
        IMemoryCache _memoryCache;

        protected static readonly List<Video> _mockVideos = new List<Video>
        {
                new Video { Id = 1, Title = "test", CreatedBy = "test@gmail.com", Views = 1 },
                new Video { Id = 2, Title = "test", CreatedBy = "test@gmail.com", Views = 5 },
                new Video { Id = 3, Title = "test", CreatedBy = "test@gmail.com", Views = 2 }
        };

        [SetUp]
        public void SetUp()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _cacheService = new CacheService<Video>(_memoryCache);
        }

        [Test]
        public void GetItems()
        {
            _cacheService.SetItems(_mockVideos, "test@gmail.com");

            var result = _cacheService.GetItems("test@gmail.com");

            result.Count.ShouldBe(3);
            result.ShouldBeOfType(typeof(List<Video>));
        }

        [Test]
        public void GetItems_GivenInvalidUsername_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => _cacheService.GetItems(""));
        }

        [Test]
        public void SetItems()
        {
            _cacheService.SetItems(_mockVideos, "test@gmail.com");
            var result = _cacheService.GetItems("test@gmail.com");

            result.Count.ShouldBe(3);
            result.ShouldBeOfType(typeof(List<Video>));
        }

        [Test]
        public void SetItems_GivenInvalidUsername_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => _cacheService.SetItems(_mockVideos, ""));
        }

        [Test]
        public void SetItems_GivenInvalidList_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => _cacheService.SetItems(new List<Video>(), "test@gmail.com"));
        }

        [Test]
        public void RemoveItems()
        {
            _cacheService.SetItems(_mockVideos, "test@gmail.com");

            _cacheService.RemoveItems("test@gmail.com");
            var result = _cacheService.GetItems("test@gmail.com");

            result.ShouldBeNull();
        }

        [Test]
        public void RemoveItems_GivenInvalidUsername_ShouldThrowArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => _cacheService.RemoveItems(""));
        }
    }
}
