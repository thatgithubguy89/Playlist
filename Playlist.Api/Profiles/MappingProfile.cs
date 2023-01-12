using AutoMapper;
using Playlist.Api.Entities;
using Playlist.Api.Entities.Dtos;

namespace Playlist.Api.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Video, VideoDto>().ReverseMap();
        }
    }
}
