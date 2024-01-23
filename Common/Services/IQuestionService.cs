using Common.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IQuestionService
    {

        public Task<QuestionResponseDto> AddQuestionAsync(QuestionDto questionDto);
        public Task<List<QuestionWithAnswersForGetDto>> GetAllQuestionsWithAnswersAsync();
        public Task<GetTrueAnswerResponseDto> GetTrueAnswer(int questionId, int answerId, int userId);
        public Task<UseHintResponseDto> UseHint(int questionId, int hintId, int userId, int? answerId = null);



    }
}
