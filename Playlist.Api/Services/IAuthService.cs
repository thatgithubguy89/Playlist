using Playlist.Api.Entities.Requests;

namespace Playlist.Api.Services
{
    public interface IAuthService
    {
        Task<string> CreateToken(AuthenticationRequest request);
        Task<bool> AuthenticateUser(AuthenticationRequest request);
    }
}
