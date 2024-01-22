using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class UserHint :BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int HintId { get; set; }
        public Hint Hint { get; set; }
        public bool IsUsed { get; set; }
    }
}
