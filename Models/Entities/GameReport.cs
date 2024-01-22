using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class GameReport : BaseEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Money { get; set; } = 0;
    }
}
