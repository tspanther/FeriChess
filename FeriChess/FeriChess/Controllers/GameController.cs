using FeriChess.Models;
using FeriChess.Interfaces;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Http;
using FeriChess.Services;

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
            var ret = new List<Field>
            {
                new Field(3, 3)
            };

            return ret;
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
        public List<FieldUpdate> MakeAMove(Move move)
        {
            var ret = new List<FieldUpdate>
            {
                new FieldUpdate(new Field(1, 1)),
                new FieldUpdate(new Piece(new Field(1, 2), true, "Queen"))
            };

            return ret;
        }
    }
}
