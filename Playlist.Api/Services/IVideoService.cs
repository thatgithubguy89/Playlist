namespace Playlist.Api.Services
{
    public interface IVideoService
    {
        Task<string> UploadVideo(IFormFile file);
        bool DeleteVideo(string path);
    }
}
