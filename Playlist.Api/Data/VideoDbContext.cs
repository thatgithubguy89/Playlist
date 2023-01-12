using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Playlist.Api.Entities;
using Playlist.Api.Entities.Common;

namespace Playlist.Api.Data
{
    public class VideoDbContext : IdentityDbContext<IdentityUser>
    {
        public VideoDbContext(DbContextOptions<VideoDbContext> options) : base(options)
        {}

        public DbSet<Video> Videos { get; set; }

        /* When an entity is added, its CreationTime and LastEditTime are set to the current time.
        When an entity is updated, its LastEditTime is set to the current time. */
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                ((BaseEntity)entity.Entity).LastEditTime = DateTime.Now;

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreationTime = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
