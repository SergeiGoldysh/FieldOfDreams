using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class JwtResponse : Response
    {
        public string? JwtToken { get; set; }
        public JwtResponse(int statusCode, string message, bool isSuccess, string jwtToken)
            : base(statusCode, message, isSuccess)
        {
            JwtToken = jwtToken;
        }

    }
}
