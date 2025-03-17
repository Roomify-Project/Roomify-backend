using Microsoft.AspNetCore.Mvc;
using Roomify.GP.API.Errors;
using Roomify.GP.Core.DTOs.User;
using Roomify.GP.Core.Services.Contract;
using System;
using System.Threading.Tasks;

namespace Roomify.GP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }



        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return user != null ? Ok(user) : NotFound(new ApiErrorResponse(404));
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateDto userDto)
        {
            var result = await _userService.UpdateUserAsync(id, userDto);
            return result ? Ok(userDto) : NotFound(new ApiErrorResponse(404));
        }


        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id == null) return BadRequest(new ApiErrorResponse(400));
            var result = await _userService.DeleteUserAsync(id);
            return result ? Ok("User Deleted Successfully") : NotFound(new ApiErrorResponse(404));
        }
    }
}
