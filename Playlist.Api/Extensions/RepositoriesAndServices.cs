using Playlist.Api.Repositories;
using Playlist.Api.Services;

namespace Playlist.Api.Extensions
{
    public static class RepositoriesAndServices
    {
        public static IServiceCollection AddRepositoriesAndServices(this IServiceCollection services)
        {
            services.AddScoped<IVideoRepository, VideoRepository>();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
            services.AddScoped<IVideoService, VideoService>();

            return services;
        }
    }
}
