using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Playlist.Function.Functions
{
    public static class ResetViews
    {
        // Runs every Sunday at midnight
        [Function("ResetViews")]
        public static async Task<HttpResponseMessage> Run([TimerTrigger("0 0 * * 0")] ILogger logger)
        {
            var httpClient = new HttpClient();

            return await httpClient.GetAsync("https://localhost:5001/api/videos/resetviews");
        }
    }
}
