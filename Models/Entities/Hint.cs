using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Hint : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? UsedByUserId { get; set; }
    }
}
