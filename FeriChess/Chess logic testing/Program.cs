using FeriChess.Models;
using FeriChess.Services;
using System;
using System.Collections.Generic;

namespace Chess_logic_testing
{
    class Program
    {

        static BoardService newBoard()
        {
            BoardService ChessBoard = new BoardService();
            for (int i = 1; i < 9; i++)
            {
                ChessBoard.AddToChessBoard(new Piece(new Field(i, 2), true, ""));
            }
            ChessBoard.AddToChessBoard(new Piece(new Field(1, 1), true, "R"));
            ChessBoard.AddToChessBoard(new Piece(new Field(8, 1), true, "R"));
            ChessBoard.AddToChessBoard(new Piece(new Field(2, 1), true, "N"));
            ChessBoard.AddToChessBoard(new Piece(new Field(7, 1), true, "N"));
            ChessBoard.AddToChessBoard(new Piece(new Field(3, 1), true, "B"));
            ChessBoard.AddToChessBoard(new Piece(new Field(6, 1), true, "B"));
            ChessBoard.AddToChessBoard(new Piece(new Field(3, 1), true, "Q"));
            ChessBoard.AddToChessBoard(new Piece(new Field(6, 1), true, "K"));
            return ChessBoard;
        }
        static List<Field> GetCoveredFields(BoardService ChessBoard, bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            foreach (var a in ChessBoard.GetChessBoard())
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
                    Console.WriteLine(ChessBoard.ListToString(ChessBoard.GetAvailableMoves(ChessBoard.GetPiece(moveFrom))));
                    input = Console.ReadLine();
                    Field moveTo = new Field(LetterToInt(input.Substring(0, 1)), Int32.Parse(input.Substring(1, 1)));
                    Move move = new Move(moveFrom, moveTo);
                    if (ChessBoard.IsValid(move))
                    {
                        ChessBoard.MakeMove(move);
                    }
                }
                catch
                {
                    Console.WriteLine("bad input");
                }

            }
        }
    }
}
