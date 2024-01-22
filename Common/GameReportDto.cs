using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class GameReportDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CorrectAnswers { get; set; }
        public decimal Money { get; set; }
        public string Massage { get; set; }


    }
}
