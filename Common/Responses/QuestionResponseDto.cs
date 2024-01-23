using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Common.Responses
{
    public class QuestionResponseDto:Response
    {
        public QuestionResponseDto(int statusCode, string message, bool isSuccess, Question question)
        : base(statusCode, message, isSuccess)
        {
            Text = question?.Text ?? "";
            Answers = question?.Answers ?? new List<Answer>();
        }
        public string Text { get; set; }
        [JsonIgnore]
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
