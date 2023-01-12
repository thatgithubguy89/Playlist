namespace Playlist.Api.Entities.Common
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastEditTime { get; set; }
    }
}
