using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Question : BaseEntity
    {
        public string Text { get; set; }
        [JsonIgnore]
        public List<Answer> Answers { get; set; } = new List<Answer>();
        
    }
}
