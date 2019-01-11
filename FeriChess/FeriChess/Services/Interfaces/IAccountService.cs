using FeriChess.Models;

namespace FeriChess.Services.Interfaces
{
    public interface IAccountService
    {
        AccountActionResponse LogIn(AuthenticationRequest authenticationRequest);
        AccountActionResponse SignIn(AuthenticationRequest authenticationRequest);
    }
}
