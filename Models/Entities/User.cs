using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string EmailUser { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public List<UserHint> UserHints { get; set; } = new List<UserHint>();
    }
}
