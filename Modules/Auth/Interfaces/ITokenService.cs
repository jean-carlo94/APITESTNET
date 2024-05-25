using APITEST.Models;

namespace APITEST.Modules.Auth.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
