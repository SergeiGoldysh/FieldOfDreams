using BL;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerApiController : ControllerBase
    {
        private readonly AnswerController _answerController;

        public AnswerApiController(AnswerController answerController)
        {
            _answerController = answerController;
        }

        [HttpPost("AddAnswer")]
        public async Task<IActionResult> AddAnswer([FromBody] AnswerDto answerDto)
        {
            if (answerDto == null)
            {
                return BadRequest("Invalid answer data");
            }

            Answer addAnswer = await _answerController.AddAnswerAsync(answerDto);
            if (addAnswer != null)
            {
                return Ok(addAnswer);
            }

            return BadRequest("Failed to add the answer");
        }
    }
}
