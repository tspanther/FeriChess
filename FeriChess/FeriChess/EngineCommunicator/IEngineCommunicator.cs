using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeriChess.EngineCommunicator
{
    public interface IEngineCommunicator
    {
        /// <summary>
        /// This function returns the next best move
        /// </summary>
        /// <param name="FEN">standart FEN notation</param>
        /// <returns>best move </returns>
        string NextMove(string FEN);
    }
}
