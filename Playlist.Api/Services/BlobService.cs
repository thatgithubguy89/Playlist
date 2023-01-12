using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;

namespace Playlist.Api.Services
{
    public class BlobService : IBlobService
    {
        private readonly IConfiguration _configuration;

        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Get the container, upload the blob to the container then return the URL for the new blob
        public async Task<string> UploadBlob(IFormFile file, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            var connectionString = _configuration["BlobConnectionString"];
            var containerName = _configuration["BlobContainerName"];

            var container = new BlobContainerClient(connectionString, containerName);
            var client = container.GetBlobClient(fileName);

            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };

            await using (Stream data = file.OpenReadStream())
            {
                await client.UploadAsync(data, httpHeaders);
            }

            return client.Uri.AbsoluteUri;
        }

        // Uses the file name to find the blob and delete it
        public async Task DeleteBlob(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            var connectionString = _configuration["BlobConnectionString"];
            var containerName = _configuration["BlobContainerName"];

            var container = new BlobContainerClient(connectionString, containerName);
            var client = container.GetBlobClient(fileName);

            await client.DeleteAsync();
        }
    }
}
