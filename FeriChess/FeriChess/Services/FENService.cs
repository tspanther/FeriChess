using FeriChess.Models;
using FeriChess.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FeriChess.Services
{
    public class FENService : IFENService
    {
        /// <summary>
        /// Generates FEN from BoardState object
        /// </summary>
        /// <param name="boardState"></param>
        /// <returns>string in FEN</returns>
        public string BoardStateToFEN(BoardState boardState)
        {
            List<List<Piece>> cb = new List<List<Piece>>();
            for (int i = 0; i < 8; i++)
            {
                List<Piece> row = new List<Piece>();
                for (int j = 0; j < 8; j++) row.Add(null);
                cb.Add(row);
            }

            foreach (Piece p in boardState.Chessboard)
            {
                cb[p.Field.Y - 1][p.Field.X - 1] = p;
            }

            cb.Reverse(); // fen goes from eigth to first row (top down for white perspective)

            StringBuilder FEN = new StringBuilder();

            // board desc
            foreach (List<Piece> row in cb)
            {
                for (int i = 0; i < 8; i++)
                {
                    int free = 0;
                    while (i < 8 && row[i] == null)
                    {
                        i++;
                        free++;
                    }
                    if (free > 0)
                    {
                        FEN.Append(free.ToString()); // # of free consecutive fields
                        i--; // to avoid incrementing i twice
                    }
                    else // piece notation
                    {
                        string piece = row[i].Name == "" ? "P" : row[i].Name[0].ToString();
                        if (!row[i].Color) // black, convert to lowercase
                        {
                            piece = piece.ToLower();
                        }
                        FEN.Append(piece);
                    }
                }
                FEN.Append('/');
            }
            FEN.Remove(FEN.Length - 1, 1); // remove last slash
            FEN.Append(' ');

            // player to move
            if (boardState.Players.Find(p => p.Color == true).Turn) FEN.Append('w');
            else FEN.Append('b');
            FEN.Append(' ');

            // possible castles
            // to do: rendering
            FEN.Append('-');
            FEN.Append(' ');

            // possible en passant
            // to do: implementation
            FEN.Append('-');
            FEN.Append(' ');

            // moves since last pawn move
            // to do: implementation
            FEN.Append('0');
            FEN.Append(' ');

            // total moves
            FEN.Append(boardState.MovesDone.Count);
            FEN.Append(' ');

            return FEN.ToString();
        }

        /// <summary>
        /// Parses FEN into BoardState object
        /// </summary>
        /// <param name="FEN"></param>
        /// <returns>BoardState described in FEN or null if invalid FEN</returns>
        public BoardState SetCustomPositionFEN(string FEN)
        {
            BoardState boardState = new BoardState()
            {
                LastMovedPiece = null,
                EnPasantHappened = false,
                EnPasantPosible = false,
                CheckPromotion = false,
                CheckChecking = false,
                TempPiece = null,
                Players = new List<Player>()
                {
                    new Player("testsubjw", true, 1000000, 0),
                    new Player("testsubjb", false, 1000000, 0)
                },
                CoveredFields = new List<Field>(),
                Chessboard = new List<Piece>(),
                Result = "",
                CastleMoves = new List<Move>(),
                MovesDone = new List<Move>()
            };

            string[] FENInfo = FEN.Split(' ');

            // pieces
            string[] boardData = FENInfo[0].Split('/');
            boardData.Reverse();

            int rw = 8;
            foreach (string row in boardData)
            {
                int pos = 0;
                for (int i = 0; i < 8; i++)
                {
                    if ((int)row[pos] >= 49 && (int)row[pos] <= 57) // number of free fields
                    {
                        i += (int)row[pos] - '0' - 1;
                    }
                    else
                    {
                        bool colour = true;
                        string pieceName = "";
                        if (row[pos] != 'p' && row[pos] != 'P') pieceName = row[pos].ToString();
                        if ((int)row[pos] > 90) // lowercase, black
                        {
                            colour = false;
                            pieceName = pieceName.ToUpper();
                        }
                        boardState.Chessboard.Add(new Piece(new Field(i + 1, rw), colour, pieceName));
                    }
                    pos++;
                }
                rw--;
            }

            // player to move
            if (FENInfo[1] == "b")
            {
                foreach (var a in boardState.Players) a.Turn = !a.Turn;
            }

            // available castles
            if (FENInfo[2] != "-")
            {
                // todo
            }

            // available en passant
            if (FENInfo[3] != "-")
            {
                // todo
            }

            // moves since last pawn move
            if (FENInfo[4] != "-")
            {
                // not implemented
            }

            // total number of moves
            if (FENInfo[5] != "-")
            {
                // not implemented
            }

            return boardState;
        }

        private bool IsValidFEN(string FEN)
        {
            string[] FENInfo = FEN.Split(' ');
            if (FENInfo.Length != 6) return false;

            /*
             *  todo...
             */

            return true;
        }
    }
}