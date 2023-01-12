using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Playlist.Test.Integration
{
    public class VideosControllerTests
    {
        Mock<IVideoRepository> _mockVideoRepository;
        Mock<IVideoService> _mockVideoService;
        Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        IMapper _mapper;

        protected static readonly IFormFile _mockFile = new FormFile(new MemoryStream(new byte[1]), 0, 1, "test", "test");
        protected static readonly Video _mockVideo = new Video { Id = 1, Title = "test", CreatedBy = "test@gmail.com", Views = 1 };
        protected static readonly List<Video> _mockVideos = new List<Video>
        {
                new Video { Id = 1, Title = "test", CreatedBy = "test@gmail.com", Views = 1 },
                new Video { Id = 2, Title = "test", CreatedBy = "test@gmail.com", Views = 5 },
                new Video { Id = 3, Title = "test", CreatedBy = "test@gmail.com", Views = 2 }
        };

        [SetUp]
        public void Setup()
        {
            _mockVideoRepository= new Mock<IVideoRepository>();
            _mockVideoService= new Mock<IVideoService>();
            _mockHttpContextAccessor= new Mock<IHttpContextAccessor>();

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test@gmail.com"),
            }, "mock"));
            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext() { User = user });

            var config = new MapperConfiguration(c => c.AddProfile(new MappingProfile()));
            _mapper = new Mapper(config);

            _mockVideoService.Setup(v => v.UploadVideo(It.IsAny<FormFile>())).Returns(Task.FromResult("test"));
        }

        [Test]
        public async Task CreateVideo()
        {
            _mockVideoRepository.Setup(v => v.AddVideoAsync(It.IsAny<FormFile>(), It.IsAny<string>())).Returns(Task.FromResult(_mockVideo));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.CreateVideo(_mockFile);
            var result = actionResult.Result as CreatedAtActionResult;
            var video = result.Value as VideoDto;

            video.Id.ShouldBe(1);
            video.ShouldBeOfType(typeof(VideoDto));
        }

        [Test]
        public async Task CreateVideo_GivenInvalidFile_ShouldReturnBadResult()
        {
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.CreateVideo(null);
            var result = actionResult.Result as BadRequestObjectResult;

            result.StatusCode.ShouldBe(400);
            result.Value.ToString().ShouldBe("File cannot be null");
        }

        [Test]
        public async Task DeleteVideo()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(_mockVideo));
            _mockVideoRepository.Setup(v => v.DeleteVideoAsync(It.IsAny<Video>()));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.DeleteVideo(1);
            var result = actionResult as OkResult;

            result.StatusCode.ShouldBe(200);
        }

        [Test]
        public async Task DeleteVideo_GivenIdForVideoThatDoesNotExist_ShouldReturnNotFound()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Video>(null));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.DeleteVideo(1);
            var result = actionResult as NotFoundObjectResult;

            result.StatusCode.ShouldBe(404);
            result.Value.ToString().ShouldBe("Video does not exist");
        }

        [Test]
        public async Task GetSingleVideo()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(_mockVideo));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.GetSingleVideo(1);
            var result = actionResult.Result as OkObjectResult;
            var video = result.Value as VideoDto;

            video.Id.ShouldBe(1);
            video.ShouldBeOfType(typeof(VideoDto));
        }

        [Test]
        public async Task GetSingleVideo_GivenIdForVideoThatDoesNotExist_ShouldReturnNotFound()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Video>(null));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.GetSingleVideo(1);
            var result = actionResult.Result as NotFoundObjectResult;

            result.StatusCode.ShouldBe(404);
            result.Value.ToString().ShouldBe("Video does not exist");
        }

        [Test]
        public async Task GetUserVideos()
        {
            _mockVideoRepository.Setup(v => v.GetAllVideosForUserAsync(It.IsAny<string>())).Returns(Task.FromResult(_mockVideos));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.GetUserVideos();
            var result = actionResult.Value;

            result.Count.ShouldBe(3);
            result.ShouldBeOfType(typeof(List<VideoDto>));
        }

        [Test]
        public async Task UpdateVideo()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(_mockVideo));
            _mockVideoRepository.Setup(v => v.UpdateVideoViewsAsync(It.IsAny<Video>())).Returns(Task.FromResult(_mockVideo));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.UpdateVideo(1);
            var result = actionResult as OkObjectResult;
            var video = result.Value as VideoDto;

            video.Id.ShouldBe(1);
            video.ShouldBeOfType(typeof(VideoDto));
        }

        [Test]
        public async Task UpdateVideo_GivenIdForVideoThatDoesNotExist_ShouldReturnNotFound()
        {
            _mockVideoRepository.Setup(v => v.GetVideoByIdAsync(It.IsAny<int>())).Returns(Task.FromResult<Video>(null));
            var videosController = new VideosController(_mockVideoRepository.Object, _mockHttpContextAccessor.Object, _mapper);

            var actionResult = await videosController.UpdateVideo(1);
            var result = actionResult as NotFoundObjectResult;

            result.StatusCode.ShouldBe(404);
            result.Value.ToString().ShouldBe("Video does not exist");
        }
    }
}
