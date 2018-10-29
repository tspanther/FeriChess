using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class Board
    {
        public List<Field> CoveredFields = new List<Field>();
        public List<Piece> ChessBoard = new List<Piece>();
        public List<Field> GetNewCoveredFields(bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            foreach (var a in ChessBoard)
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
            return ChessBoard;
        }
        public void AddToChessBoard(Piece p)
        {
            ChessBoard.Add(p);
        }
        public override string ToString()
        {
            string s = "";
            foreach(var a in ChessBoard)
            {
                s += a.ToString()+" ";
            }
            return s;
        }
        public List<Move> GetPawnAttack(Piece p)
        {
            List<Move> AvailableMoves = new List<Move>();
            Field newField;
            if (p.Color)//white
            {
                if (p.F.X + 1 <= 8 && p.F.Y + 1 <= 8)
                {
                    newField = new Field(p.F.X + 1, p.F.Y + 1);
                    AvailableMoves.Add(new Move(p.F, newField));
                }
                if (p.F.X - 1 > 0 && p.F.Y + 1 <= 8)
                {
                    newField = new Field(p.F.X - 1, p.F.Y + 1);
                    AvailableMoves.Add(new Move(p.F, newField));
                }
            }
            else //black
            {
                if (p.F.X + 1 <= 8 && p.F.Y - 1 > 0)
                {
                    newField = new Field(p.F.X + 1, p.F.Y - 1);
                    AvailableMoves.Add(new Move(p.F, newField));
                }
                if (p.F.X -1 > 0 && p.F.Y - 1 > 0)
                {
                    newField = new Field(p.F.X - 1, p.F.Y - 1);
                    AvailableMoves.Add(new Move(p.F, newField));
                }
            }
            return AvailableMoves;
        }
        public List<Field> Around(Piece p)
        {
            List<Field> moves = new List<Field>();
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (p.F.X + i<=8&& p.F.X + i>0&& p.F.Y + j<=8&& p.F.Y + j>0)
                        moves.Add(new Field(p.F.X+i,p.F.Y+ j));
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
                        if (!ChessBoard.Exists(x => x.F.Y - 1 == p.F.Y && x.F.X == p.F.X))
                        {
                            newField = new Field(p.F.X, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField)); //move 1
                        }
                        if (p.F.Y == 2 && !ChessBoard.Exists(x => x.F.Y - 2 == p.F.Y))
                        {
                            newField = new Field(p.F.X, p.F.Y + 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//move 2
                        if (ChessBoard.Exists(x => x.F.Y - 1 == p.F.Y && x.F.X - 1 == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 1, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }//capture
                        if (ChessBoard.Exists(x => x.F.Y - 1 == p.F.Y && x.F.X + 1 == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 1, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }//capture
                    }
                    else //black
                    {
                        if (!ChessBoard.Exists(x => x.F.Y + 1 == p.F.Y))
                        {
                            newField = new Field(p.F.X, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField)); //move 1
                        }
                        if (p.F.Y == 7 && !ChessBoard.Exists(x => x.F.Y + 2 == p.F.Y))
                        {
                            newField = new Field(p.F.X, p.F.Y - 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//move 2
                        if (ChessBoard.Exists(x => x.F.Y + 1 == p.F.Y && x.F.X - 1 == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 1, p.F.Y - 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        if (ChessBoard.Exists(x => x.F.Y + 1 == p.F.Y && x.F.X + 1 == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 1, p.F.Y - 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                    }
                    break;
                case "K":
                    CoveredFields = GetNewCoveredFields(!p.Color);
                    foreach(var a in Around(p))
                    {
                        if (ChessBoard.Exists(x => x.F.X == a.X && x.F.Y == a.Y && x.Color != p.Color) && !Covered(a))
                        {
                            AvailableMoves.Add(new Move(p.F, a));
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.X == a.X && x.F.Y == a.Y && x.Color != p.Color) && Covered(a)) continue;
                        AvailableMoves.Add(new Move(p.F, a));
                    }

                    break;
                case "Q":
                    for (int i = p.F.Y + 1; i <= 8; i++)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X, i);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color == p.Color)) break;
                        newField = new Field(p.F.X, i);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//up
                    for (int i = p.F.Y - 1; i > 0; i--)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X, i);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color == p.Color)) break;
                        newField = new Field(p.F.X, i);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//down
                    for (int i = p.F.X - 1; i > 0; i--)
                    {
                        if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.F.Y);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.F.Y);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//left
                    for (int i = p.F.X + 1; i <= 8; i++)
                    {
                        if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.F.Y);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.F.Y);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//right
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.F.X + i < 8 && p.F.Y + i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X + i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X + i, p.F.Y + i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//up right
                        if (p.F.X - i > 0 && p.F.Y + i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X - i, p.F.Y + i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//up left
                        if (p.F.X - i > 0 && p.F.Y - i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y - i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X - i, p.F.Y - i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//down left
                        if (p.F.X + i < 8 && p.F.Y - i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X + i, p.F.Y - i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//down right
                    }//diagonals
                    break;
                case "B":
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.F.X + i < 8 && p.F.Y + i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X + i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X + i, p.F.Y + i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//up right
                        if (p.F.X - i > 0 && p.F.Y + i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y + i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X - i, p.F.Y + i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//up left
                        if (p.F.X - i > 0 && p.F.Y - i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y - i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X - i && x.F.Y == p.F.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X - i, p.F.Y - i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//down left
                        if (p.F.X + i < 8 && p.F.Y - i < 8)
                        {
                            if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.F.X - i, p.F.Y + i);
                                AvailableMoves.Add(new Move(p.F, newField));
                                break;
                                //todo capture
                            }
                            else if (ChessBoard.Exists(x => x.F.X == p.F.X + i && x.F.Y == p.F.Y - i && x.Color == p.Color)) break;
                            newField = new Field(p.F.X + i, p.F.Y - i);
                            AvailableMoves.Add(new Move(p.F, newField));
                        }//down right
                    }//diagonals
                    break;
                case "N":
                    if (p.F.Y + 1 < 8 && p.F.X + 2 < 8)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y + 1 && x.F.X == p.F.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 2, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X + 2, p.F.Y + 1);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.Y + 2 < 8 && p.F.X + 1 < 8)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y + 2 && x.F.X == p.F.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 1, p.F.Y + 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X + 1, p.F.Y + 2);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.Y - 2 > 0 && p.F.X + 1 < 8)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y - 2 && x.F.X == p.F.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 1, p.F.Y - 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X + 1, p.F.Y - 2);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.X - 2 > 0 && p.F.Y + 1 < 8)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y + 1 && x.F.X == p.F.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 2, p.F.Y + 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X - 2, p.F.Y + 1);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.X + 2 < 8 && p.F.Y - 1 > 0)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y - 1 && x.F.X == p.F.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X + 2, p.F.Y - 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X + 2, p.F.Y - 1);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.Y + 2 < 8 && p.F.X - 1 > 0)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y + 2 && x.F.X == p.F.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 1, p.F.Y + 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X - 1, p.F.Y + 2);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.Y - 1 > 0 && p.F.X - 2 > 0)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y - 1 && x.F.X == p.F.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 2, p.F.Y - 1);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X - 2, p.F.Y - 1);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    if (p.F.Y - 2 > 0 && p.F.X - 1 > 0)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == p.F.Y - 2 && x.F.X == p.F.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X - 1, p.F.Y - 2);
                            AvailableMoves.Add(new Move(p.F, newField));
                            //todo capture
                        }
                        newField = new Field(p.F.X - 1, p.F.Y - 2);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }
                    break;
                case "R":
                    for (int i = p.F.Y + 1; i <= 8; i++)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X, i);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color == p.Color)) break;
                        newField = new Field(p.F.X, i);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//up
                    for (int i = p.F.Y - 1; i > 0; i--)
                    {
                        if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color != p.Color))
                        {
                            newField = new Field(p.F.X, i);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.Y == i && x.F.X == p.F.X && x.Color == p.Color)) break;
                        newField = new Field(p.F.X, i);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//down
                    for (int i = p.F.X - 1; i > 0; i--)
                    {
                        if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.F.Y);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.F.Y);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//left
                    for (int i = p.F.X + 1; i <= 8; i++)
                    {
                        if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.F.Y);
                            AvailableMoves.Add(new Move(p.F, newField));
                            break;
                            //todo capture
                        }
                        else if (ChessBoard.Exists(x => x.F.X == i && x.F.Y == p.F.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.F.Y);
                        AvailableMoves.Add(new Move(p.F, newField));
                    }//right
                    break;
            }
            return AvailableMoves;
        }
        public string ListToString(List<Move> l)
        {
            string s="";
            foreach(var a in l)
            {
                s += a.To.ToString()+"";
            }
            return s;
        }
    }
}