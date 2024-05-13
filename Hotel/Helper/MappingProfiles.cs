using AutoMapper;
using Hotel.Dto;
using Hotel.Models;

namespace Hotel.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<User, UserDto>();
            CreateMap<Place, PlaceDto>();
            CreateMap<UserDto, User>();
            CreateMap<PlaceDto, Place>();
            CreateMap<User, UserWithPlacesDto>();
            CreateMap<UserWithPlacesDto, User>();
        }
    }
}
