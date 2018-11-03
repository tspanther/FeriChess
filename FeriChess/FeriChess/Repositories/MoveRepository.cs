using FeriChess.Models;
using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Repositories
{
    public class MoveRepository
    {
        IDatabase db = new Database("connection string...");

        public List<Move> GetMoves()
        {
            /*
             * also supports linq & lambda expressions
             * read more here https://github.com/schotime/NPoco
             */
            List<Move> moves = db.Fetch<Move>("select * from movesTable");
            return moves;
        }
    }
}