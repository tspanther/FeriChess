using FeriChess.Models;
using Newtonsoft.Json.Linq;
using System.Web.Http;

namespace FeriChess.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        [Route("get-available-moves")]
        [HttpPost]
        public string GetAvailableMoves(Field field)
        {
            return string.Format("get-available-moves: {0} {1}", field.X, field.Y);
        }

        [Route("make-a-move")]
        [HttpPost]
        public string MakeAMove(Move move)
        {
            return string.Format("make-a-move: from: {0} {1} | to: {2} {3}", move.From.X, move.From.Y, move.To.X, move.To.Y);
        }
    }
}
