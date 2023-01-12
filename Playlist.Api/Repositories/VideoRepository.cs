using Microsoft.EntityFrameworkCore;
using Playlist.Api.Data;
using Playlist.Api.Entities;
using Playlist.Api.Services;

namespace Playlist.Api.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly VideoDbContext _context;
        private readonly IVideoService _videoService;
        private readonly ICacheService<Video> _cacheService;

        public VideoRepository(VideoDbContext context, IVideoService videoService, ICacheService<Video> cacheService)
        {
            _context = context;
            _videoService = videoService;
            _cacheService = cacheService;
        }

        public async Task<Video> AddVideoAsync(IFormFile file, string username)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var video = new Video
            {
                Title = file.FileName,
                VideoPath = await _videoService.UploadVideo(file),
                CreatedBy = username
            };

            var createdVideo = await _context.Videos.AddAsync(video);
            await _context.SaveChangesAsync();

            _cacheService.RemoveItems(username);

            return createdVideo.Entity;
        }

        public async Task DeleteVideoAsync(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video));

            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();

            _videoService.DeleteVideo(video.VideoPath);

            _cacheService.RemoveItems(video.CreatedBy);
        }

        public async Task<List<Video>> GetAllVideosForUserAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            var videos = new List<Video>();

            videos = _cacheService.GetItems(username);
            if (videos != null)
                return videos;

            videos = _context.Videos
                .Where(v => v.CreatedBy == username)
                .OrderByDescending(v => v.Views).ToList();

            _cacheService.SetItems(videos, username);

            return videos;
        }

        public async Task<Video> GetVideoByIdAsync(int id)
        {
            return await _context.Videos.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task ResetAllViewsAsync()
        {
            var videos = await _context.Videos
                .Where(v => v.Views > 0)
                .ToListAsync();

            foreach (var video in videos)
            {
                video.Views = 0;
            }

            _context.Videos.UpdateRange(videos);
            await _context.SaveChangesAsync();
        }

        public async Task<Video> UpdateVideoViewsAsync(Video video)
        {
            if (video == null)
                throw new ArgumentNullException(nameof(video));

            _context.Entry(video).State = EntityState.Detached;

            video.Views++;

            _context.Videos.Update(video);
            await _context.SaveChangesAsync();

            return video;
        }
    }
}
