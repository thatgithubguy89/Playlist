namespace Playlist.Api.Services
{
    public interface IBlobService
    {
        Task<string> UploadBlob(IFormFile file, string fileName);
        Task DeleteBlob(string fileName);
    }
}
