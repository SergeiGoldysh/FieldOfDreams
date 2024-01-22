using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class UseHintResponseDto: Response
    {
        public UseHintResponseDto(int statusCode, string message, bool isSuccess, UseHintDto useHintDto)
        : base(statusCode, message, isSuccess)
        {
            QuestionId = useHintDto?.QuestionId ?? 0;
            Text = useHintDto?.Text ?? "";
            Text = useHintDto?.Text ?? "";
            Answers = useHintDto?.Answers ?? new List<AnswerForHintGetDto>();
        }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        //public string Massage { get; set; }
        public List<AnswerForHintGetDto> Answers { get; set; } = new List<AnswerForHintGetDto>();
    }
}
