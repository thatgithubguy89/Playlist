namespace Playlist.Api.Services
{
    public class VideoService : IVideoService
    {
        private readonly IWebHostEnvironment _environment;

        public VideoService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // Set the filename, create a path to wwwroot/videos/filename and send the file contents to it
        public async Task<string> UploadVideo(IFormFile file)
        {
            if (string.IsNullOrEmpty(file.FileName)) throw new ArgumentNullException();
            
            var fileName = Guid.NewGuid().ToString() + file.FileName;
            var path = Path.Combine(_environment.WebRootPath, "videos/", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            return "/videos/" + fileName;
        }

        // Check to see if the video exists, then delete it
        public bool DeleteVideo(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            var newPath = _environment.WebRootPath + path;

            if (File.Exists(newPath))
            {
                File.Delete(newPath);
                return true;
            }
            
            return false;
        }
    }
}
