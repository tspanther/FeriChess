using FeriChess.Models;
using FeriChess.Services.Interfaces;
using System.Collections.Generic;
using System.Web.Http;

namespace FeriChess.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
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
                return accountService.LogIn(authenticationRequest);
            }
            else if (authenticationRequest.Type == AuthenticationRequest.AuthenticationType.Signin)
            {
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

