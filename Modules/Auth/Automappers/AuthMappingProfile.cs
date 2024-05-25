using APITEST.Models;
using APITEST.Modules.Auth.DTOs;
using AutoMapper;

namespace APITEST.Modules.Auth.Automappers
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile() {
            CreateMap<AuthDto, User>();
            CreateMap<User, AuthDto>();
        }
    }
}
