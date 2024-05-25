using APITEST.Common.Interfaces;
using APITEST.Data;
using APITEST.Models;
using Microsoft.EntityFrameworkCore;

namespace APITEST.Modules.Users.Repository
{
    public class UserRepository : IRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll() => 
            await _context.Users.ToListAsync();

        public async Task<User> GetById(int id) => 
            await _context.Users.FindAsync(id);

        public async Task Create(User user) => 
            await _context.Users.AddAsync(user);

        public void Update(User user)
        {
            _context.Users.Attach(user);
            _context.Users.Entry(user).State = EntityState.Modified;
        }

        public void Delete(User user) => _context.Users.Remove(user);

        public async Task Save() => 
            await _context.SaveChangesAsync();

        public IEnumerable<User> Search(Func<User, bool> filter) => 
            _context.Users.Where(filter).ToList();
        public User SearchOne(Func<User, bool> filter) =>
            _context.Users.Where(filter).FirstOrDefault();

    }
}
