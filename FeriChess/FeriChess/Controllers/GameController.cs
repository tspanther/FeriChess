using FeriChess.Models;
using FeriChess.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using FeriChess.Services;
using System.Linq;

namespace FeriChess.Controllers
{
    [RoutePrefix("api/game")]
    public class GameController : ApiController
    {
        private IBoardService _boardService;

        public GameController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        /// <summary>
        /// Accepts Field object on route: api/game/get-available-moves
        /// Returns list of Fields: available moves for client to render
        /// If no move is available, returns empty list
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [Route("get-available-moves")]
        [HttpPost]
        public List<Field> GetAvailableMoves(Field field)
        {
            return _boardService.GetAvailableMoves(field).Select(m => new Field(m.To.X, m.To.Y)).ToList();
        }

        /// <summary>
        /// Accepts Move object on route: api/game/make-a-move
        /// Returns list of FieldUpdate objects: fields with new values that were changed during the move.
        /// Returns empty list if move is invalid.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        [Route("make-a-move")]
        [HttpPost]
        public bool MakeAMove(Move move)
        {
            return _boardService.IsValid(move);
        }
    }
}
