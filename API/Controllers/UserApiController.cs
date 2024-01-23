using BL;
using Common;
using Common.Responses;
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
            UserResponse userResponse = await _userController.AddUserAsync(userDto);
            if (userResponse == null)
            {
                return BadRequest("not found");
            }
            return Ok(userResponse);
        }

        [HttpPost("AuthorizeUser")]
        public async Task<IActionResult> AuthorizeUser([FromBody] UserAuthorizeDto userAuthorizeDto)
        {
            if (userAuthorizeDto == null)
            {
                return BadRequest("Invalid question data");
            }
            UserResponse userResponse = await _userController.AuthorizeUserAsync(userAuthorizeDto);
            if (userResponse == null)
            {
                return BadRequest("not found");
            }
            return Ok(userResponse);
        }
    }
}
