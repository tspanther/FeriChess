using FeriChess.Models;
using FeriChess.Services.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FeriChess.Services
{
    public class AccountService: IAccountService
    {
        private Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public List<Account> Accounts { get; set; }
        public AccountService()
        {
            Accounts = new List<Account>()
            {
                new Account()
                {
                    Email = "name@domain.com",
                    Password = "123chess123",
                    Username = "janeznovak123"
                },
                new Account()
                {
                    Email = "name2@domain.com",
                    Password = "chess123chess",
                    Username = "johndoe123"
                }
            };
        }

        public AccountActionResponse LogIn(AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest.Username != null)
            {
                if (!Accounts.Exists(x => x.Username == authenticationRequest.Username))
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.LoginAccountNonExist,
                        Successful = false
                    };
                }
                if (Accounts.Find(x => x.Username == authenticationRequest.Username).Password != authenticationRequest.Password)
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.LoginIncorrectPassword,
                        Successful = false
                    };
                }
            }
            else
            {
                if (!Accounts.Exists(x => x.Email != authenticationRequest.Email))
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.LoginAccountNonExist,
                        Successful = false
                    };
                }
                if (Accounts.Find(x => x.Email == authenticationRequest.Email).Password != authenticationRequest.Password)
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.LoginIncorrectPassword,
                        Successful = false
                    };
                }
            }
            return new AccountActionResponse()
            {
                Response = AccountActionResponse.Responses.LoginSuccess,
                Successful = true
            };
        }

        public AccountActionResponse SignIn(AuthenticationRequest authenticationRequest)
        {
            if (!isValidEmail(authenticationRequest.Email) || !isValidUsername(authenticationRequest.Username)
                || Accounts.Exists(x => x.Email == authenticationRequest.Email) || Accounts.Exists(x => x.Username == authenticationRequest.Username))
            {
                return new AccountActionResponse()
                {
                    Response = AccountActionResponse.Responses.SigninInvalidUserNameOrEmail,
                    Successful = true
                };
            }

            Accounts.Add(new Account()
            {
                Email = authenticationRequest.Email,
                Username = authenticationRequest.Username,
                Password = authenticationRequest.Password
            });

            return new AccountActionResponse()
            {
                Response = AccountActionResponse.Responses.SigninSuccess,
                Successful = true
            };
        }

        private bool isValidUsername(string username)
        {
            return username.Length >= 2;
        }

        private bool isValidEmail(string email)
        {
            Match match = emailRegex.Match(email);
            return match.Success;
        }
    }
}