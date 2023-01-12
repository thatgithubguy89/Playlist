using Microsoft.AspNetCore.Hosting;

namespace Playlist.Test.Unit
{
    public class VideoServiceTests
    {
        Mock<IWebHostEnvironment> _environment;
        IVideoService _videoService;
        protected static readonly IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");

        [SetUp]
        public void Setup()
        {
            _environment = new Mock<IWebHostEnvironment>();
            _environment.Setup(e => e.WebRootPath).Returns("C:\\Projects\\Playlist\\Playlist.Api\\wwwroot\\");
            _videoService = new VideoService(_environment.Object);
        }

        [Test]
        [Ignore("")]
        public async Task UploadVideo()
        {
            var path = await _videoService.UploadVideo(_mockFile);
            path = $"C:\\Projects\\Playlist\\Playlist.Api\\wwwroot\\{path}";

            var result = Path.Exists(path);

            result.ShouldBeTrue();
        }

        [Test]
        public async Task UploadVideo_GivenInvalidFilename_ShouldReturnArgumentNullException()
        {
            var file = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "");

            await Should.ThrowAsync<ArgumentNullException>(async () => await _videoService.UploadVideo(file));
        }

        [Test]
        [Ignore("")]
        public async Task DeleteVideo()
        {
            var path = await _videoService.UploadVideo(_mockFile);

            var result = _videoService.DeleteVideo(path);

            result.ShouldBeTrue();
            path = $"C:\\Projects\\Playlist\\Playlist.Api\\wwwroot\\{path}";
            Path.Exists(path).ShouldBeFalse(path);
        }

        [Test]
        public void DeleteVideo_GivenPathThatDoesNotExist_ShouldReturnFalse()
        {
            var result = _videoService.DeleteVideo("/videos/wrongpath");

            result.ShouldBeFalse();
        }

        [Test]
        public void DeleteVideo_GivenInvalidPath_ShouldReturnArgumentNullException()
        {
            Should.Throw<ArgumentNullException>(() => _videoService.DeleteVideo(""));
        }
    }
}
