using APITEST.Common.Interfaces;
using APITEST.Data;
using APITEST.Models;
using APITEST.Modules.Users.DTOs;
using Microsoft.EntityFrameworkCore;

namespace APITEST.Modules.Users.Services
{
    public class UserService : ICommonService<UserDto, UserInserDto, UserUpdateDto>
    {
        private AppDbContext _context;
        public UserService(AppDbContext context) { 
            _context = context;
        }
        public async Task<IEnumerable<UserDto>> FindAll()
        {
            var users = await _context.Users.Select(user => new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            }).ToListAsync();

            return users;
        }

        public async Task<UserDto> FindOneById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                var userDto = new UserDto()
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                };

                return userDto;
            }

            return null;
        }

        public async Task<UserDto> CreateUser(UserInserDto userInsertDto)
        {
            var user = new User()
            {
                Name = userInsertDto.Name,
                Email = userInsertDto.Email,
                Password = userInsertDto.Password,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            return userDto;
        }
        public async Task<UserDto> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return null;
            }

            user.Name = userUpdateDto.Name;
            user.Email = userUpdateDto.Email;
            user.Password = userUpdateDto.Password;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            var userDto = new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
            };

            return userDto;
        }

        public async Task<Boolean> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
