using APITEST.Modules.Users.DTOs;

namespace APITEST.Common.Interfaces
{
    public interface ICommonService<T, TI, TU>
    {
        Task<IEnumerable<T>> FindAll();
        Task<T> FindOneById(int id);
        Task<T> CreateUser(TI user);
        Task<T> UpdateUser(int id, TU user);
        Task<bool> DeleteUserAsync(int id);
    }
}
