using FeriChess.Models;
using FeriChess.Services;
using System;
using System.Collections.Generic;

namespace Chess_logic_testing
{
    class Program
    {

        static int LetterToInt(string s)
        {
            int t = s[0];
            t -= 96;
            return t;
        }
        static void Main(string[] args)
        {
            string input;
            Player p1 = new Player("a", false, 1000, 0);
            Player p2 = new Player("b", true, 1000, 0);
            BoardService ChessBoard = new BoardService(p1,p2);
            while (true)
            {
                Console.WriteLine(ChessBoard.ToString());
                input = Console.ReadLine();
                try
                {
                    Field moveFrom = new Field(LetterToInt(input.Substring(0, 1)), Int32.Parse(input.Substring(1, 1)));
                    Console.WriteLine(ChessBoard.ListToString(ChessBoard.GetAvailableMoves(moveFrom)));
                    input = Console.ReadLine();
                    Field moveTo = new Field(LetterToInt(input.Substring(0, 1)), Int32.Parse(input.Substring(1, 1)));
                    Move move = new Move(moveFrom, moveTo);
                    ChessBoard.IsValid(move);
                }
                catch
                {
                    Console.WriteLine("bad input");
                }

            }
        }
    }
}
