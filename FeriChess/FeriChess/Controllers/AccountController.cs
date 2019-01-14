using FeriChess.Models;
using FeriChess.Repositories;
using FeriChess.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace FeriChess.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        MoveRepository baza = new MoveRepository("creativepowercell.asuscomm.com", "Uporabnik", "FeriChess");
        public IAccountService accountService { get; private set; }

        public AccountController(IAccountService _accountService)
        {
            accountService = _accountService;
        }

        [Route("authenticate")]
        [HttpPost]
        public AccountActionResponse Authenticate(AuthenticationRequest authenticationRequest)
        {
            if (authenticationRequest == null)
            {
                return new AccountActionResponse()
                {
                    Response = AccountActionResponse.Responses.AuthenticationUnsuccessful,
                    Successful = false
                };
            }
            if (authenticationRequest.Type == AuthenticationRequest.AuthenticationType.Login)
            {
                if (baza.ObstajaIgralec(authenticationRequest.Username, authenticationRequest.Password))
                { 
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.LoginSuccess,
                        Successful = true
                    };
                }

                return accountService.LogIn(authenticationRequest);
            }
            else if (authenticationRequest.Type == AuthenticationRequest.AuthenticationType.Signin)
            {
                if (baza.DodajIgralca(authenticationRequest.Username, authenticationRequest.Password))
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.SigninSuccess,
                        Successful = true
                    };
                }
                else
                {
                    return new AccountActionResponse()
                    {
                        Response = AccountActionResponse.Responses.SigninInvalidUserNameOrEmail,
                        Successful = false
                    };
                }

                return accountService.SignIn(authenticationRequest);
            }

            return new AccountActionResponse()
            {
                Response = AccountActionResponse.Responses.AuthenticationUnsuccessful,
                Successful = false
            };
        }
    }
}

