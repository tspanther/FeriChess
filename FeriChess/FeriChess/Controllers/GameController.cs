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

        [Route("load-boardstate/{id}")]
        [HttpGet]
        public List<FieldUpdate> LoadBoardstate(int id = 0)
        {
            List<FieldUpdate> ret = _boardService.LoadBoardstate(id);
            return ret;
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
        /// Returns empty list inside GameStateChange object if move is invalid.
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        [Route("make-a-move")]
        [HttpPost]
        public GamestateChange MakeAMove(Move move)
        {
            if (_boardService.IsValid(move))
            {
                GamestateChange gsc = _boardService.GetFieldUpdates(move);
                //if (_boardService.isComputerOpponent)
                //{
                //    GamestateChange gscE = _boardService.GetFieldUpdates(null);
                //    gscE.UpdateFields.AddRange(gsc.UpdateFields);
                //    return gscE;
                //}
                return gsc;
            }
            return new GamestateChange
            {
                UpdateFields = new List<FieldUpdate>(),
                GameResult = ""
            };
        }
    }
}
