using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UseHintDto
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public string Massage { get; set; }
        public List<AnswerForHintGetDto> Answers { get; set; } = new List<AnswerForHintGetDto>();
    }
}
