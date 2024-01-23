using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class QuestionDto
    {
        public string Text { get; set; }

        [MinLength(4, ErrorMessage = "At least 4 answers are required.")]
        public List<AnswerWithQuestionDto> Answers { get; set; } = new List<AnswerWithQuestionDto>();

    }
}
