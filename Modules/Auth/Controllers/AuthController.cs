using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FluentValidation;
using APITEST.Modules.Auth.DTOs;
using APITEST.Modules.Auth.Interfaces;

namespace APITEST.Modules.Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IValidator<AuthLoginDto> _authLoginValidator;
        private readonly IValidator<AuthRegisterDto> _authRegisterValidator;
        private readonly IAuthService _authService;

        public AuthController(
            IValidator<AuthLoginDto> authLoginValidator,
            IValidator<AuthRegisterDto> authRegisterValidator,
            IAuthService authService
        ) 
        { 
            _authLoginValidator = authLoginValidator;
            _authRegisterValidator = authRegisterValidator;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<AuthDto>> Register(AuthRegisterDto authRegisterDto)
        {
            var validationResult = await _authRegisterValidator.ValidateAsync(authRegisterDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_authService.Validate(authRegisterDto))
            {
                return BadRequest(_authService.Errors);
            }

            var user = await _authService.Register(authRegisterDto);

            return CreatedAtAction(nameof(Check), new { id = user.Id }, user);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthDto>> Login(AuthLoginDto authLoginDto)
        {
            var validationResult = await _authLoginValidator.ValidateAsync(authLoginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_authService.Validate(authLoginDto))
            {
                return Unauthorized(_authService.Errors);
            }

            var login = await _authService.Login(authLoginDto);
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
