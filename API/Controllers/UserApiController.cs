using BL;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Repositories.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserController _userController;

        public UserApiController(UserController userController)
        {
            _userController = userController;
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Invalid question data");
            }
            User user = await _userController.AddUserAsync(userDto);
            if (user == null)
            {
                return BadRequest("not found");
            }
            return Ok(user);
        }
    }
}
