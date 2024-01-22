using Common;
using Models.Entities;
using Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class AnswerController
    {
        private readonly IRepository<Answer> _answerRepository;

        public AnswerController(IRepository<Answer> answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<Answer> AddAnswerAsync(AnswerDto answerDto)
        {
            Answer answer = new Answer
            {
                Text = answerDto.Text,
                IsCorrect = answerDto.IsCorrect,
                QuestionId = answerDto.QuestionId
            };

            return await _answerRepository.AddAsync(answer);
        }
    }
}
