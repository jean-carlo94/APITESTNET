using APITEST.Common.Interfaces;
using APITEST.Models;
using APITEST.Modules.Auth.DTOs;
using APITEST.Modules.Auth.Interfaces;
using APITEST.Modules.Users.DTOs;
using APITEST.Modules.Users.Services;
using APITEST.Modules.Users.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APITEST.Modules.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<AuthLoginDto> _authLoginValidator;
        private readonly IAuthService _authService;
        private readonly ICommonService<UserDto, UserInsertDto, UserUpdateDto> _userService;

        public AuthController(
            IValidator<AuthLoginDto> authLoginValidator,
            IAuthService authService,
            [FromKeyedServices("userService")] ICommonService<UserDto, UserInsertDto, UserUpdateDto> userService
        ) 
        { 
            _authLoginValidator = authLoginValidator;
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthDto>> Login(AuthLoginDto loginDto)
        {
            var validationResult = await _authLoginValidator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_authService.Validate(loginDto))
            {
                return Unauthorized(_authService.Errors);
            }

            var login = await _authService.Login(loginDto);
            return login;
        }

        [HttpGet("Check")]
        [Authorize]
        public async Task<ActionResult<AuthDto>> Check()
        {
            // Retrieve userId from the claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized("No se encontro ID");
            }

            var authDto = await _authService.Check(userIdClaim);

            if (authDto == null)
            {
                return Unauthorized("Usuario no encontrado");
            }

            return Ok(authDto);
        }
    }
}
