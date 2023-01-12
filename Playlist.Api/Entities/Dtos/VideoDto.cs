using Playlist.Api.Entities.Common;

namespace Playlist.Api.Entities.Dtos
{
    public class VideoDto : BaseEntity
    {
        public string? Title { get; set; }
        public string? VideoPath { get; set; }
        public int Views { get; set; }
    }
}
