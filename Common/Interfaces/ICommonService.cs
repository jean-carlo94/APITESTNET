using APITEST.Modules.Users.DTOs;

namespace APITEST.Common.Interfaces
{
    public interface ICommonService<T, TIN, TUP>
    {
        public List<string> Errors { get; }
        Task<IEnumerable<T>> FindAll();
        Task<T> FindById(int id);
        Task<T> Create(TIN user);
        Task<T> Update(int id, TUP user);
        Task<T> Delete(int id);
        bool Validate(TIN dto);
        bool Validate(TUP dto);
    }
}
