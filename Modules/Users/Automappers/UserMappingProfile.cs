using APITEST.Common.Utils;
using APITEST.Models;
using APITEST.Modules.Users.DTOs;
using AutoMapper;

namespace APITEST.Modules.Users.Automappers
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
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserInsertDto, User>()
                .ForMember(user => user.Password,
                            dto => dto.ConvertUsing(new PasswordToHashConverter(), dto => dto.Password)
                )
                .ForMember(user => user.Email,
                            dto => dto.ConvertUsing(new EmailToLower(), dto => dto.Email)
                );

            CreateMap<User, UserDto>();

            CreateMap<UserUpdateDto, User>()
                .ForMember(user => user.Password,
                            dto => dto.ConvertUsing(new PasswordToHashConverter(), dto => dto.Password)
                )
                .ForMember(user => user.Email,
                            dto => dto.ConvertUsing(new EmailToLower(), dto => dto.Email)
                );
        }
    }
}
