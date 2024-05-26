using APITEST.Common.Utils;
using APITEST.Models;
using APITEST.Modules.Auth.DTOs;
using AutoMapper;

namespace APITEST.Modules.Auth.Automappers
{
    public class AuthMappingProfile : Profile
    {
        public class PasswordToHashConverter : IValueConverter<string, string>
        {
            public string Convert(string sourceMember, ResolutionContext context)
            {
                return PasswordHash.Hash(sourceMember);
            }
        }

        public class EmailToLower : IValueConverter<string, string>
        {
            public string Convert(string sourceMember, ResolutionContext context)
            {
                return sourceMember.ToLower();
            }
        }
        public AuthMappingProfile() {
            CreateMap<AuthDto, User>();
            CreateMap<User, AuthDto>();
            CreateMap<AuthRegisterDto, User>()
                .ForMember(user => user.Password,
                            dto => dto.ConvertUsing(new PasswordToHashConverter(), dto => dto.Password)
                )
                .ForMember(user => user.Email,
                            dto => dto.ConvertUsing(new EmailToLower(), dto => dto.Email)
                );
        }
    }
}
