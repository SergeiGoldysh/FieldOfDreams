using Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class QuestionWithAnswersForGetDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        
        public List<AnswerForGetDto> Answers { get; set; } = new List<AnswerForGetDto>();
        
    }
}
