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
            BoardService ChessBoard = new BoardService();
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
