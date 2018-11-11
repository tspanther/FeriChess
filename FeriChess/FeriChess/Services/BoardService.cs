using FeriChess.Models;
using FeriChess.Interfaces;
using System.Collections.Generic;

namespace FeriChess.Services
{
    public class BoardService: IBoardService
    {
        public BoardService()
        {
            Chessboard = SetStartingPosition();
        }
        public List<Field> CoveredFields = new List<Field>();
        public List<Piece> Chessboard = new List<Piece>();
        public List<Piece> SetStartingPosition()
        {
            List<Piece> board = new List<Piece>();
            for (int i = 1; i < 9; i++)
            {
                board.Add(new Piece(new Field(i, 2), true, ""));
            }
            board.Add(new Piece(new Field(1, 1), true, "R"));
            board.Add(new Piece(new Field(8, 1), true, "R"));
            board.Add(new Piece(new Field(2, 1), true, "N"));
            board.Add(new Piece(new Field(7, 1), true, "N"));
            board.Add(new Piece(new Field(3, 1), true, "B"));
            board.Add(new Piece(new Field(6, 1), true, "B"));
            board.Add(new Piece(new Field(3, 1), true, "Q"));
            board.Add(new Piece(new Field(6, 1), true, "K"));
            for (int i = 1; i < 9; i++)
            {
                board.Add(new Piece(new Field(i, 7), false, ""));
            }
            board.Add(new Piece(new Field(1, 8), false, "R"));
            board.Add(new Piece(new Field(8, 8), false, "R"));
            board.Add(new Piece(new Field(2, 8), false, "N"));
            board.Add(new Piece(new Field(7, 8), false, "N"));
            board.Add(new Piece(new Field(3, 8), false, "B"));
            board.Add(new Piece(new Field(6, 8), false, "B"));
            board.Add(new Piece(new Field(3, 8), false, "Q"));
            board.Add(new Piece(new Field(6, 8), false, "K"));
            return board;
        }
        public List<Field> GetNewCoveredFields(bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            foreach (var a in Chessboard)
            {
                if (a.Color == Color)
                {
                    if (a.Name == "")
                    {
                        foreach (var b in GetPawnAttack(a))
                        {
                            if (!CoveredFields.Exists(x => x.X == b.To.X && x.Y == b.To.Y))
                            {
                                CoveredFields.Add(b.To);
                            }
                        }
                        continue;
                    }
                    foreach (var b in GetAvailableMoves(a))
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
        public List<Piece> GetChessBoard()
        {
            return Chessboard;
        }
        public void AddToChessBoard(Piece p)
        {
            Chessboard.Add(p);
        }
        public override string ToString()
        {
            string s = "";
            foreach (var a in Chessboard)
            {
                s += a.ToString() + " ";
            }
            return s;
        }
        //public void Capture(Field f)
        //{
        //    ChessBoard.Remove()
        //}
        public List<Move> GetPawnAttack(Piece p)
        {
            List<Move> AvailableMoves = new List<Move>();
            Field newField;
            if (p.Color)//white
            {
                if (p.Field.X + 1 <= 8 && p.Field.Y + 1 <= 8)
                {
                    newField = new Field(p.Field.X + 1, p.Field.Y + 1);
                    AvailableMoves.Add(new Move(p.Field, newField));
                }
                if (p.Field.X - 1 > 0 && p.Field.Y + 1 <= 8)
                {
                    newField = new Field(p.Field.X - 1, p.Field.Y + 1);
                    AvailableMoves.Add(new Move(p.Field, newField));
                }
            }
            else //black
            {
                if (p.Field.X + 1 <= 8 && p.Field.Y - 1 > 0)
                {
                    newField = new Field(p.Field.X + 1, p.Field.Y - 1);
                    AvailableMoves.Add(new Move(p.Field, newField));
                }
                if (p.Field.X - 1 > 0 && p.Field.Y - 1 > 0)
                {
                    newField = new Field(p.Field.X - 1, p.Field.Y - 1);
                    AvailableMoves.Add(new Move(p.Field, newField));
                }
            }
            return AvailableMoves;
        }
        public List<Field> Around(Piece p)
        {
            List<Field> moves = new List<Field>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (p.Field.X + i <= 8 && p.Field.X + i > 0 && p.Field.Y + j <= 8 && p.Field.Y + j > 0)
                        moves.Add(new Field(p.Field.X + i, p.Field.Y + j));
                }
            }
            return moves;
        }
        public bool Covered(Field f)
        {
            if (CoveredFields.Exists(x => x.X == f.X && x.Y == f.Y)) return true;
            return false;
        }
        public List<Move> GetAvailableMoves(Piece p)
        {
            List<Move> AvailableMoves = new List<Move>();
            Field newField;
            switch (p.Name)
            {
                case "":
                    if (p.Color)//white
                    {
                        if (!Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField)); //move 1
                        }
                        if (p.Field.Y == 2 && !Chessboard.Exists(x => x.Field.Y - 2 == p.Field.Y&& x.Field.X==p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//move 2
                        if (Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X - 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }//capture
                        if (Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X + 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }//capture
                    }
                    else //black
                    {
                        if (!Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField)); //move 1
                        }
                        if (p.Field.Y == 7 && !Chessboard.Exists(x => x.Field.Y + 2 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//move 2
                        if (Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X - 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        if (Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X + 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                    }
                    break;
                case "K":
                    CoveredFields = GetNewCoveredFields(!p.Color);
                    foreach (var a in Around(p))
                    {
                        if (Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color != p.Color) && !Covered(a))
                        {
                            AvailableMoves.Add(new Move(p.Field, a));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color != p.Color) && Covered(a)) continue;
                        AvailableMoves.Add(new Move(p.Field, a));
                    }

                    break;
                case "Q":
                    for (int i = p.Field.Y + 1; i <= 8; i++)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//up
                    for (int i = p.Field.Y - 1; i > 0; i--)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//down
                    for (int i = p.Field.X - 1; i > 0; i--)
                    {
                        if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//left
                    for (int i = p.Field.X + 1; i <= 8; i++)
                    {
                        if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//right
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i < 8 && p.Field.Y + i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X + i, p.Field.Y + i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//up right
                        if (p.Field.X - i > 0 && p.Field.Y + i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X - i, p.Field.Y + i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//up left
                        if (p.Field.X - i > 0 && p.Field.Y - i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X - i, p.Field.Y - i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//down left
                        if (p.Field.X + i < 8 && p.Field.Y - i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X + i, p.Field.Y - i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//down right
                    }//diagonals
                    break;
                case "B":
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i < 8 && p.Field.Y + i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X + i, p.Field.Y + i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//up right
                        if (p.Field.X - i > 0 && p.Field.Y + i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X - i, p.Field.Y + i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//up left
                        if (p.Field.X - i > 0 && p.Field.Y - i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X - i, p.Field.Y - i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//down left
                        if (p.Field.X + i < 8 && p.Field.Y - i < 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.Field.X + i, p.Field.Y - i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//down right
                    }//diagonals
                    break;
                case "N":
                    if (p.Field.Y + 1 <= 8 && p.Field.X + 2 <= 8)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if(Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X + 2 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y + 2 <= 8 && p.Field.X + 1 <= 8)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X + 1 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 2 > 0 && p.Field.X + 1 <= 8)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X + 1 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.X - 2 > 0 && p.Field.Y + 1 <= 8)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X - 2 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.X + 2 <= 8 && p.Field.Y - 1 > 0)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if(Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X + 2 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y + 2 <= 8 && p.Field.X - 1 > 0)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X - 1 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 1 > 0 && p.Field.X - 2 > 0)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X - 2 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 2 > 0 && p.Field.X - 1 > 0)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X - 2 && x.Color == p.Color));
                        else
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    break;
                case "R":
                    for (int i = p.Field.Y + 1; i <= 8; i++)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//up
                    for (int i = p.Field.Y - 1; i > 0; i--)
                    {
                        if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//down
                    for (int i = p.Field.X - 1; i > 0; i--)
                    {
                        if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//left
                    for (int i = p.Field.X + 1; i <= 8; i++)
                    {
                        if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                            //todo capture
                        }
                        else if (Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//right
                    break;
            }
            return AvailableMoves;
        }
        public void MakeMove(Move m)
        {
            if (Chessboard.Exists(x => x.Field.X == m.To.X && x.Field.Y == m.To.Y))
            {
                Chessboard.Remove(Chessboard.Find(x => x.Field.X == m.To.X && x.Field.Y == m.To.Y)); //capture
            }
            Chessboard.Find(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y).Field.X = m.To.X;
            Chessboard.Find(x => x.Field.X == m.To.X && x.Field.Y == m.From.Y).Field.Y = m.To.Y;
        }
        public bool IsValid(Move m)
        {
            if (!Chessboard.Exists(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y)) return false; //validates if piece exists at desired from field
            if (!GetAvailableMoves(Chessboard.Find(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y)).Exists(x => x.To.X == m.To.X && x.To.Y == m.To.Y)) return false; //validates if move is possible
            return true;
        }
        public Piece GetPiece(Field f)
        {
            return Chessboard.Find(x => x.Field.X == f.X && x.Field.Y == f.Y);
        }
        public string ListToString(List<Move> l)
        {
            string s = "";
            foreach (var a in l)
            {
                s += a.To.ToString() + "";
            }
            return s;
        }
    }
}