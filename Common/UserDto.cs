using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string EmailUser { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
