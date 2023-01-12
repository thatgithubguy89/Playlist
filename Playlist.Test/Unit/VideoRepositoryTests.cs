using Playlist.Api.Services;

namespace Playlist.Test.Unit
{
    public class VideoRepositoryTests
    {
        IVideoRepository _videoRepository;
        Mock<IVideoService> _mockVideoService;
        Mock<ICacheService<Video>> _mockCacheService;
        VideoDbContext _context;
        DbContextOptions<VideoDbContext> _contextOptions = new DbContextOptionsBuilder<VideoDbContext>()
            .UseInMemoryDatabase("VideoRepositoryTests")
            .Options;

        protected static readonly IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");
        protected static readonly Video _mockVideo = new Video { Id = 1, Title = "test", CreatedBy = "test@gmail.com", Views = 1 };
        protected static readonly List<Video> _mockVideos = new List<Video>
        {
                new Video { Id = 1, Title = "test", CreatedBy = "test@gmail.com", Views = 1 },
                new Video { Id = 2, Title = "test", CreatedBy = "test@gmail.com", Views = 5 },
                new Video { Id = 3, Title = "test", CreatedBy = "shouldnotbeselected@gmail.com" }
        };

        [SetUp]
        public void Setup()
        {
            _context = new VideoDbContext(_contextOptions);

            _mockCacheService= new Mock<ICacheService<Video>>();
            _mockCacheService.Setup(c => c.SetItems(It.IsAny<List<Video>>(), It.IsAny<string>()));
            _mockCacheService.Setup(c => c.GetItems(It.IsAny<string>()));
            _mockCacheService.Setup(c => c.RemoveItems(It.IsAny<string>()));

            _mockVideoService = new Mock<IVideoService>();
            _mockVideoService.Setup(v => v.UploadVideo(It.IsAny<FormFile>())).Returns(Task.FromResult("test"));

            _videoRepository = new VideoRepository(_context, _mockVideoService.Object, _mockCacheService.Object);
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void Teardown()
        {
            _context.ChangeTracker.Clear();
            _context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddVideoAsync()
        {
            var result = await _videoRepository.AddVideoAsync(_mockFile, "test@gmail.com");

            result.Id.ShouldBe(1);
            result.Title.ShouldBe("test");
            result.VideoPath.ShouldBe("test");
        }

        [Test]
        public async Task AddVideoAsync_GivenInvalidFile_ShouldThrowArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () => await _videoRepository.AddVideoAsync(null, "test@gmail.com"));
        }

        [Test]
        public async Task DeleteVideoAsync()
        {
            await _videoRepository.AddVideoAsync(_mockFile, "test@gmail.com");
            _context.ChangeTracker.Clear();

            await _videoRepository.DeleteVideoAsync(_mockVideo);
            var result = await _context.Videos.FirstOrDefaultAsync(v => v.Id == 1);

            result.ShouldBeNull();
        }

        [Test]
        public async Task DeleteVideoAsync_GivenInvalidVideo_ShouldThrowArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () => await _videoRepository.DeleteVideoAsync(null));
        }

        [Test]
        public async Task GetAllVideosForUserAsync()
        {
            await _context.Videos.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetAllVideosForUserAsync("test@gmail.com");

            result.Count.ShouldBe(2);
            result.ShouldBeOfType(typeof(List<Video>));
            result[0].Id.ShouldBe(2);
        }

        [Test]
        public async Task GetVideoByIdAsync()
        {
            await _context.Videos.AddAsync(_mockVideo);
            await _context.SaveChangesAsync();

            var result = await _videoRepository.GetVideoByIdAsync(1);

            result.Id.ShouldBe(1);
            result.ShouldBeOfType(typeof(Video));
        }

        [Test]
        public async Task GetAllVideosForUserAsync_GivenInvalidUsername_ShouldThrowArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () => await _videoRepository.GetAllVideosForUserAsync(""));
        }

        [Test]
        public async Task ResetAllViewsAsync()
        {
            await _context.Videos.AddRangeAsync(_mockVideos);
            await _context.SaveChangesAsync();

            await _videoRepository.ResetAllViewsAsync();
            var result = await _videoRepository.GetAllVideosForUserAsync("test@gmail.com");

            result[0].Views.ShouldBe(0);
            result[1].Views.ShouldBe(0);
        }

        [Test]
        public async Task UpdateVideoViewsAsync()
        {
            await _videoRepository.AddVideoAsync(_mockFile, "test@gmail.com");
            _context.ChangeTracker.Clear();

            var result = await _videoRepository.UpdateVideoViewsAsync(_mockVideo);

            result.Id.ShouldBe(1);
            result.Views.ShouldBe(2);
            result.ShouldBeOfType(typeof(Video));
        }

        [Test]
        public async Task UpdateVideoViewsAsync_GivenInvalidVideo_ShouldThrowArgumentNullException()
        {
            await Should.ThrowAsync<ArgumentNullException>(async () => await _videoRepository.UpdateVideoViewsAsync(null));
        }
    }
}
