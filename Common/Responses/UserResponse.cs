using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class UserResponse: JwtResponse
    {
        public UserResponse(int statusCode, string message, bool isSuccess, string jwtToken, User user)
        : base(statusCode, message, isSuccess, jwtToken)
        {
            Id = user?.Id ?? 0;
            UserName = user?.UserName ?? "";
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        [JsonIgnore]
        public List<UserHint> UserHints { get; set; } = new List<UserHint>();
    }
}
