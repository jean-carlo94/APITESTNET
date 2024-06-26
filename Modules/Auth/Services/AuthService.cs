﻿using AutoMapper;
using APITEST.Common.Interfaces;
using APITEST.Common.Utils;
using APITEST.Models;
using APITEST.Modules.Auth.DTOs;
using APITEST.Modules.Auth.Interfaces;

namespace APITEST.Modules.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenservice;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        public List<string> Errors { get; }

        public AuthService(
            ITokenService tokenService,
            IRepository<User> userRepository,
            IMapper mapper
        ) {
            _tokenservice = tokenService;
            _userRepository = userRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<AuthDto> Register(AuthRegisterDto registerDto) {
            var user = _mapper.Map<User>(registerDto);

            await _userRepository.Create(user);
            await _userRepository.Save();

            var userDto = _mapper.Map<AuthDto>(user);
            return userDto;
        }

        public async Task<AuthDto> Login(AuthLoginDto loginDto)
        {
            var user = _userRepository.SearchOne(user => user.Email == loginDto.Email);
            var authDto = _mapper.Map<AuthDto>(user);

            authDto.Token = _tokenservice.CreateToken(user);

            return authDto;
        }

        public async Task<AuthDto> Check(string seachId) 
        {
            var Id = int.Parse(seachId);
            var user = await _userRepository.GetById(Id);

            if (user == null) {
                return null;
            }

            var authDto = _mapper.Map<AuthDto>(user);

            return authDto;
        }

        public bool Validate(AuthRegisterDto authRegisterDto)
        {
            if (
                _userRepository.Search(
                    user => user.Email == authRegisterDto.Email.ToLower()
                )
                .Count() > 0
            )
            {
                Errors.Add("El email ya ha sido utilizado");
                return false;
            }
            return true;
        }

        public bool Validate(AuthLoginDto loginDto) {
            var user = _userRepository.SearchOne(user => user.Email == loginDto.Email);

            if (user == null)
            {
                Errors.Add("Error de usuario o contraseña");
                return false;
            }

            if (!PasswordHash.Verify(loginDto.Password, user.Password))
            {
                Errors.Add("Error de usuario o contraseña");
                return false;
            }

            return true;
        }
    }
}
