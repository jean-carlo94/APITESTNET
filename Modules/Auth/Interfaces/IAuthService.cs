
using APITEST.Modules.Auth.DTOs;

namespace APITEST.Modules.Auth.Interfaces
{
    public interface IAuthService
    {
        public List<string> Errors { get; }
        Task<AuthDto> Login(AuthLoginDto loginDto);

        Task<AuthDto> Check(string seachId);

        bool Validate(AuthLoginDto dto);
    }
}
