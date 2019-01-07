using FeriChess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeriChess.Services.Interfaces
{
    public interface IFENService
    {
        /// <summary>
        /// Generates FEN from BoardState object
        /// </summary>
        /// <param name="boardState"></param>
        /// <returns>string in FEN</returns>
        string BoardStateToFEN(BoardState boardState);

        /// <summary>
        /// Parses FEN into BoardState object
        /// </summary>
        /// <param name="FEN"></param>
        /// <returns>BoardState described in FEN or null if invalid FEN</returns>
        BoardState SetCustomPositionFEN(string FEN);
    }
}
