using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class AccountActionResponse
    {
        public static class Responses
        {
            public const string LoginSuccess = "Logged in successfully";
            public const string LoginIncorrectPassword = "Incorrect password";
            public const string LoginAccountNonExist = "Account does not exist! Have you already signed in?";

            public const string SigninSuccess = "Signed in successfully. Welcome";
            public const string SigninInvalidUserNameOrEmail = "Username or email invalid or already in use.";

            public const string AuthenticationUnsuccessful = "Authentication was unsuccessful";
        }

        public string Response { get; set; }
        public bool Successful { get; set; }
    }
}