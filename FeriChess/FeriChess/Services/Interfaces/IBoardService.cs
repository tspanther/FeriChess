using FeriChess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeriChess.Interfaces
{
    public interface IBoardService
    {
        /// <summary>
        /// Stores this
        /// </summary>
        //List<Piece> ChessBoard { get; set; }
        // ^ commented out because you still only have fields instead of properties in BoardService implementation

        /// <summary>
        /// Does that
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        List<Move> GetAvailableMoves(Piece p);

        /*
         * ... and more method definitions to come
         */
    }
}
