using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APITEST.Data;
using APITEST.Models;
using APITEST.Modules.Users.DTOs;
using FluentValidation;
using APITEST.Common.Interfaces;

namespace APITEST.Modules.Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IValidator<UserInserDto> _userInsertValidator;
        private readonly IValidator<UserUpdateDto> _userUpdateValidator;
        private readonly ICommonService<UserDto, UserInserDto, UserUpdateDto> _userService;

        public UsersController(
            IValidator<UserInserDto> userInsertValidator,
            IValidator<UserUpdateDto> userUpdateValidator,
            ICommonService<UserDto, UserInserDto, UserUpdateDto> userService
         )
        {
            _userInsertValidator = userInsertValidator;
            _userUpdateValidator = userUpdateValidator;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetUsers() => await _userService.FindAll();

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id) {
            var user = await _userService.FindOneById(id);

            return user == null ? 
                        NotFound() 
                        : 
                        Ok(user);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserInserDto userInsertDto)
        {
            var validationResult = await _userInsertValidator.ValidateAsync(userInsertDto);

            if (!validationResult.IsValid) {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userService.CreateUser(userInsertDto);

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // PUT | PATCH: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [HttpPatch("{id}")]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            var validationResult = await _userUpdateValidator.ValidateAsync(userUpdateDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _userService.UpdateUser(id, userUpdateDto);

            return user == null ?
                        NotFound()
                        :
                        Ok(user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var delete = await _userService.DeleteUserAsync(id);

            return !delete ? NotFound() : NoContent();
        }
    }
}
