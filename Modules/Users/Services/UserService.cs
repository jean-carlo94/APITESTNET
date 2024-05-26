using Microsoft.EntityFrameworkCore;
using AutoMapper;
using APITEST.Common.Interfaces;
using APITEST.Models;
using APITEST.Modules.Users.DTOs;

namespace APITEST.Modules.Users.Services
{
    public class UserService: ICommonService<UserDto, UserInsertDto, UserUpdateDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public UserService(
            IRepository<User> userRepository,
            IMapper mapper
        ) { 
            _userRepository = userRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }
        public async Task<IEnumerable<UserDto>> FindAll()
        {
            var users = await _userRepository.GetAll();

            return users.Select(user => _mapper.Map<UserDto>(user));
        }

        public async Task<UserDto> FindById(int Id)
        {
            var user = await _userRepository.GetById(Id);

            if (user != null)
            {
                var userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }

            return null;
        }

        public async Task<UserDto> Create(UserInsertDto userInsertDto)
        {
            var user = _mapper.Map<User>(userInsertDto);

            await _userRepository.Create(user);
            await _userRepository.Save();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        public async Task<UserDto> Update(int Id, UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetById(Id);

            if (user == null)
            {
                return null;
            }

            user = _mapper.Map<UserUpdateDto, User>(userUpdateDto, user);

            _userRepository.Update(user);

            try
            {
                await _userRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto> Delete(int Id)
        {
            var user = await _userRepository.GetById(Id);

            if (user == null)
            {
                return null;
            }

            _userRepository.Delete(user);
            await _userRepository.Save();

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public bool Validate(UserInsertDto userInserDto)
        {
            if (
                _userRepository.Search(
                    user => user.Email == userInserDto.Email
                )
                .Count() > 0 
            )
            {
                Errors.Add("El email ya ha sido utilizado");
                return false;
            }
            return true;
        }

        public bool Validate(UserUpdateDto userUpdateDto)
        {
            if (_userRepository.Search(
                user => user.Email == userUpdateDto.Email 
                && userUpdateDto.Id != user.Id                
                )
                .Count() > 0
            )
            {
                Errors.Add("El email ya ha sido utilizado");
                return false;
            }
            return true;
        }

    }
}
