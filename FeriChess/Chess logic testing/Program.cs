using FeriChess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess_logic_testing
{
    class Program
    {
        static bool IsValid(Move m,Board ChessBoard)
        { 
            if (!ChessBoard.GetChessBoard().Exists(x => x.F.X == m.From.X && x.F.Y == m.From.Y)) return false; //validates if piece exists at desired from field
            if (!ChessBoard.GetAvailableMoves(ChessBoard.GetChessBoard().Find(x => x.F.X == m.From.X && x.F.Y == m.From.Y)).Exists(x => x.To.X == m.To.X && x.To.Y == m.To.Y)) return false; //validates if move is possible
            return true;
        }
        static Board newBoard()
        {
            Board ChessBoard = new Board();
            for (int i = 1; i < 9; i++)
            {
                ChessBoard.AddToChessBoard(new Piece(new Field(i, 2), true, ""));
            }
            ChessBoard.AddToChessBoard(new Piece(new Field(5, 8), false, "K"));
            return ChessBoard;
        }
        static List<Field> GetCoveredFields(Board ChessBoard,bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            foreach(var a in ChessBoard.GetChessBoard())
            {
                if (a.Color == Color)
                {
                    if (a.Name == "")
                    {
                        foreach (var b in ChessBoard.GetPawnAttack(a))
                        {
                            if (!CoveredFields.Exists(x => x.X == b.To.X && x.Y == b.To.Y))
                            {
                                CoveredFields.Add(b.To);
                            }
                        }
                        continue;
                    }
                    foreach (var b in ChessBoard.GetAvailableMoves(a))
                    {
                        if (!CoveredFields.Exists(x => x.X == b.To.X && x.Y == b.To.Y))
                        {
                            CoveredFields.Add(b.To);
                        }
                    }
                }
            }
            return CoveredFields;
        }
        static string ListToString(List<Field> l)
        {
            string s = "";
            foreach (var a in l)
            {
                s += a.ToString() + " ";
            }
            return s;
        }
        static void Main(string[] args)
        {
            Board ChessBoard = newBoard();
            Console.WriteLine(ChessBoard.ToString());
            Console.WriteLine(ChessBoard.ListToString(ChessBoard.GetAvailableMoves(ChessBoard.GetChessBoard().Find (x=>x.F.X == 5 && x.F.Y == 8))));
        }
    }
}
