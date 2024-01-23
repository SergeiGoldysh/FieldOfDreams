using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class GetTrueAnswerResponseDto:Response
    {
        public GetTrueAnswerResponseDto(int statusCode, string message, bool isSuccess, GetTrueAnswerDto getTrueAnswerDto)
        : base(statusCode, message, isSuccess)
        {
            AnswerId = getTrueAnswerDto?.Id ?? 0;
            QuestionId = getTrueAnswerDto?.QuestionId ?? 0;
            Text = getTrueAnswerDto?.Text ?? "";
            Massage = getTrueAnswerDto?.Massage ?? "";
            IsCorrect = getTrueAnswerDto?.IsCorrect ?? false;
        }
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public string? Massage { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
