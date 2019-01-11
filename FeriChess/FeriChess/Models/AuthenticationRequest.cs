using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class AuthenticationRequest
    {
        public enum AuthenticationType
        {
            Login, Signin
        };

        public AuthenticationType Type { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}