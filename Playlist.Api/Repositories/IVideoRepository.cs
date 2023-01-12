using Playlist.Api.Entities;

namespace Playlist.Api.Repositories
{
    public interface IVideoRepository
    {
        Task<Video> AddVideoAsync(IFormFile file, string username);
        Task DeleteVideoAsync(Video video);
        Task<List<Video>> GetAllVideosForUserAsync(string username);
        Task<Video> GetVideoByIdAsync(int id);
        Task ResetAllViewsAsync();
        Task<Video> UpdateVideoViewsAsync(Video video);
    }
}
