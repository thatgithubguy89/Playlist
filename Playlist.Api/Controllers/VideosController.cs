using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Playlist.Api.Entities;
using Playlist.Api.Entities.Dtos;
using Playlist.Api.Repositories;

namespace Playlist.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public VideosController(IVideoRepository videoRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<VideoDto>>> GetUserVideos()
        {
            try
            {
                var username = _httpContextAccessor.HttpContext.User.Claims.First().Value;

                var videos = await _videoRepository.GetAllVideosForUserAsync(username);

                return _mapper.Map<List<VideoDto>>(videos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDto>> GetSingleVideo(int id)
        {
            try
            {
                var video = await _videoRepository.GetVideoByIdAsync(id);
                if (video == null)
                    return NotFound("Video does not exist");

                return Ok(_mapper.Map<VideoDto>(video));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Video>> CreateVideo(IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest("File cannot be null");

                var createdVideo = await _videoRepository.AddVideoAsync(file, "test@gmail.com");

                var video = _mapper.Map<VideoDto>(createdVideo);

                return CreatedAtAction(nameof(GetSingleVideo),
                    new { id = video.Id }, video);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateVideo(int id)
        {
            try
            {
                var video = await _videoRepository.GetVideoByIdAsync(id);
                if (video == null)
                    return NotFound("Video does not exist");

                var updatedVideo = await _videoRepository.UpdateVideoViewsAsync(video);

                return Ok(_mapper.Map<VideoDto>(updatedVideo));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVideo(int id)
        {
            try
            {
                var video = await _videoRepository.GetVideoByIdAsync(id);
                if (video == null)
                    return NotFound("Video does not exist");

                await _videoRepository.DeleteVideoAsync(video);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("resetviews")]
        public async Task<ActionResult> ResetAllVideoViews()
        {
            try
            {
                await _videoRepository.ResetAllViewsAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
