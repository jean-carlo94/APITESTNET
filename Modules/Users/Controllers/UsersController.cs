using Microsoft.AspNetCore.Mvc;
using APITEST.Modules.Users.DTOs;
using FluentValidation;
using APITEST.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace APITEST.Modules.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IValidator<UserInsertDto> _userInsertValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;
        private readonly ICommonService<UserDto, UserInsertDto, UserUpdateDto> _userService;

        public UsersController(
            IValidator<UserInsertDto> userInsertValidator,
            IValidator<UserUpdateDto> userUpdateValidator,
            [FromKeyedServices("userService")] ICommonService<UserDto, UserInsertDto, UserUpdateDto> userService
         )
        {
            _userInsertValidator = userInsertValidator;
            _userUpdateValidator = userUpdateValidator;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<UserDto>> GetUsers() => await _userService.FindAll();

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id) {
            var user = await _userService.FindById(id);

            return user == null ? 
                        NotFound() 
                        : 
                        Ok(user);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<UserDto>> CreateUser(UserInsertDto userInsertDto)
        {
            var validationResult = await _userInsertValidator.ValidateAsync(userInsertDto);

            if (!validationResult.IsValid) {
                return BadRequest(validationResult.Errors);
            }

            if (!_userService.Validate(userInsertDto))
            {
                return BadRequest(_userService.Errors);
            }

            var user = await _userService.Create(userInsertDto);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT | PATCH: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            var validationResult = await _userUpdateValidator.ValidateAsync(userUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userService.Update(id, userUpdateDto);

            if (!_userService.Validate(userUpdateDto))
            {
                return BadRequest(_userService.Errors);
            }

            return user == null ?
                        NotFound()
                        :
                        Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var delete = await _userService.Delete(id);

            return delete == null ? NotFound() : NoContent();
        }
    }
}
