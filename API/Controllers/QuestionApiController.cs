using BL;
using Common;
using Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Repositories.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionApiController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionApiController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpPost("AddQuestion")]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto questionDto)
        {
            var question = await _questionService.AddQuestionAsync(questionDto);

            if (question == null)
            {
                return BadRequest("Invalid question data");
            }

            return Ok(question);
        }

        [HttpGet("GetAllQuestionsWithAnswers")]
        public async Task<IActionResult> GetAllQuestionsWithAnswers()
        {
            var questionsWithAnswers = await _questionService.GetAllQuestionsWithAnswersAsync();
            return Ok(questionsWithAnswers);
        }
        [HttpGet("getTrueAnswer")]
        public async Task<IActionResult> GetTrueAnswer(int questionId, int answerId, int userId)
        {
            var trueAnswerDto = await _questionService.GetTrueAnswer(questionId, answerId, userId);

            if (trueAnswerDto == null)
            {
                return NotFound();
            }

            return Ok(trueAnswerDto);
        }

        [HttpGet("UseHint")]
        public async Task<IActionResult> UseHint(int questionId, int hintId, int userId, int? answerId = null)
        {
            var useHint = await _questionService.UseHint(questionId, hintId, userId, answerId);

            if (useHint == null)
            {
                return NotFound();
            }

            return Ok(useHint);
        }
    }
}
