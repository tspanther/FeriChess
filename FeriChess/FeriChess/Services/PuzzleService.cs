using FeriChess.Models;
using FeriChess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Services
{
    public class PuzzleService
    {
        public string getpuzzle(int id)
        {
            MoveRepository database = new MoveRepository("creativepowercell.asuscomm.com", "Uporabnik", "FeriChess");
            return database.GetPuzzle(id);
        } // http://wtharvey.com/m8n2.txt
    }
}