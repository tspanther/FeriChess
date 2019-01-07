using FeriChess.Models;
using FeriChess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FeriChess.EngineCommunicator;
using System.Text;

namespace FeriChess.Services
{
    public class BoardService : IBoardService
    {
        private static List<string> puzzles = new List<string>()
        {
            "8/8/8/1r1B3R/3K1k2/8/8/8 b - - 0 175",
            "r2qkb1r/pp2nppp/3p4/2pNN1B1/2BnP3/3P4/PPP2PPP/R2bK2R w KQkq - 1 0",
            "1rb4r/pkPp3p/1b1P3n/1Q6/N3Pp2/8/P1P3PP/7K w - - 1 0",
            "4kb1r/p2n1ppp/4q3/4p1B1/4P3/1Q6/PPP2PPP/2KR4 w k - 1 0",
            "r1b2k1r/ppp1bppp/8/1B1Q4/5q2/2P5/PPP2PPP/R3R1K1 w - - 1 0",
            "5rkr/pp2Rp2/1b1p1Pb1/3P2Q1/2n3P1/2p5/P4P2/4R1K1 w - - 1 0",
            "1r1kr3/Nbppn1pp/1b6/8/6Q1/3B1P2/Pq3P1P/3RR1K1 w - - 1 0"
        }; // http://wtharvey.com/m8n2.txt

        private static IEngineCommunicator engineCommunicator;
        public bool isComputerOpponent { get; set; }

        public BoardService(IEngineCommunicator _engineCommunicator)
        {
            Players.Add(new Player("testsubjw", true, 1000000, 0));
            Players.Add(new Player("testsubjb", false, 1000000, 0));
            engineCommunicator = _engineCommunicator;
            isComputerOpponent = false;
            SetStartingPosition();
        }

        public void SetCustomPosition(string s)
        {
            Chessboard = new List<Piece>();
            string[] pieces = s.Split(',');
            foreach (var a in pieces)
            {
                Chessboard.Add(new Piece(new Field(a[2], a[3] - '0'), a[0] == 1 ? true : false, a[1].ToString()));
            }
        }

        public void SetCustomPositionFEN(string FEN)
        {
            Chessboard = new List<Piece>();
            Players = new List<Player>()
            {
                new Player("testsubjw", true, 1000000, 0),
                new Player("testsubjb", false, 1000000, 0)
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
                        Chessboard.Add(new Piece(new Field(i+1, rw), colour, pieceName));
                    }
                    pos++;
                }
                rw--;
            }

            // player to move
            if (FENInfo[1] == "b")
            {
                ChangeTurn();
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
        }
        public string MovesToString()
        {
            string s="";
            foreach(Move a in MovesDone)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        public string BoardStateToFEN()
        {
            List<List<Piece>> cb = new List<List<Piece>>();
            for (int i=0; i<8; i++)
            {
                List<Piece> row = new List<Piece>();
                for (int j = 0; j < 8; j++) row.Add(null);
                cb.Add(row);
            }

            foreach (Piece p in Chessboard)
            {
                cb[p.Field.Y-1][p.Field.X-1] = p;
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
            if (Players.Find(p => p.Color == true).Turn) FEN.Append('w');
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
            FEN.Append(MovesDone.Count);
            FEN.Append(' ');

            return FEN.ToString();
        }
        public string GameStateToString()
        {
            string s="";
            foreach(var a in Chessboard)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        public void SetCustomPosition(string s)
        {
            Chessboard = new List<Piece>();
            string[] pieces = s.Split(',');
            foreach (var a in pieces)
            {
                Chessboard.Add(new Piece(new Field(a[2],a[3]-'0'),a[0]==1?true:false,a[1].ToString()));
            }
        }
        public string MovesToString()
        {
            string s="";
            foreach(Move a in MovesDone)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        public string GameStateToString()
        {
            string s="";
            foreach(var a in Chessboard)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        private Piece LastMovedPiece;
        private bool EnPasantHappened = false;
        private bool EnPasantPosible=false;
        private bool CheckPromotion = false;
        private bool checkChecking = false;
        private Piece tempPiece=null;
        private List<Player> Players = new List<Player>();
        private List<Field> CoveredFields = new List<Field>();
        private List<Piece> Chessboard = new List<Piece>();
        private string result = "";
        private List<Move> CastleMoves= new List<Move>();
        private List<Move> MovesDone = new List<Move>();
        public void SetStartingPosition()
        {
            Chessboard = new List<Piece>();
            for (int i = 1; i < 9; i++)
            {
                Chessboard.Add(new Piece(new Field(i, 2), true, ""));
            }
            Chessboard.Add(new Piece(new Field(1, 1), true, "R"));
            Chessboard.Add(new Piece(new Field(8, 1), true, "R"));
            Chessboard.Add(new Piece(new Field(2, 1), true, "N"));
            Chessboard.Add(new Piece(new Field(7, 1), true, "N"));
            Chessboard.Add(new Piece(new Field(3, 1), true, "B"));
            Chessboard.Add(new Piece(new Field(6, 1), true, "B"));
            Chessboard.Add(new Piece(new Field(4, 1), true, "Q"));
            Chessboard.Add(new Piece(new Field(5, 1), true, "K"));
            for (int i = 1; i < 9; i++)
            {
                Chessboard.Add(new Piece(new Field(i, 7), false, ""));
            }
            Chessboard.Add(new Piece(new Field(1, 8), false, "R"));
            Chessboard.Add(new Piece(new Field(8, 8), false, "R"));
            Chessboard.Add(new Piece(new Field(2, 8), false, "N"));
            Chessboard.Add(new Piece(new Field(7, 8), false, "N"));
            Chessboard.Add(new Piece(new Field(3, 8), false, "B"));
            Chessboard.Add(new Piece(new Field(6, 8), false, "B"));
            Chessboard.Add(new Piece(new Field(4, 8), false, "Q"));
            Chessboard.Add(new Piece(new Field(5, 8), false, "K"));
        }
        private void MovePromotion(Move m)
        {
            string Choice = "Q";
            Chessboard.Add(new Piece(new Field(m.To.X, m.To.Y), ActivePlayer().Color, Choice));
            Chessboard.Remove(GetPiece(m.From));
            CheckPromotion = false;
            
        }
        private List<Field> GetNewCoveredFields(bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            List<Move> temp;
            foreach (var a in Chessboard.ToList())
            {
                if (a.Color != Color)
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
                    else if (a.Name == "K")
                    {
                        foreach (var b in Around(a))
                        {
                            if (!CoveredFields.Exists(x => x.X == b.X && x.Y == b.Y))
                            {
                                CoveredFields.Add(b);
                            }
                        }
                    }
                    else
                    {
                        temp = GetAvailableMoves(a.Field);
                        foreach (var b in temp)
                        {
                            if (!CoveredFields.Exists(x => x==b.To))
                            {
                                CoveredFields.Add(b.To);
                            }
                        }
                    }
                }
            }
            return CoveredFields;
        }
        private List<Piece> GetChessBoard()
        {
            return Chessboard;
        }
        private void AddToChessBoard(Piece p)
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
        private List<Move> GetPawnAttack(Piece p)
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
        private List<Field> Around(Piece p)
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
        private bool Covered(Field f)
        {
            if (CoveredFields.Exists(x => x.X == f.X && x.Y == f.Y)) return true;
            return false;
        }
        private Player ActivePlayer()
        {   
            return Players.Find(x => x.Turn == true);
        }
        private Player InactivePlayer()
        {
            return Players.Find(x => x.Turn == false);
        }
        public Field GetKingPos(bool color)
        {
            return Chessboard.Find(x => x.Name == "K" && x.Color == color).Field;
        }
        private bool MovesPossible()
        {
            foreach (var a in Chessboard.ToList())
            {
                if (a.Color != ActivePlayer().Color) continue;
                if (GetAvailableMoves(a.Field).Count != 0)
                {
                    return true;
                }
            }
            return false;
        }
        public List<Move> GetAvailableMoves(Field f)
        {
            Piece p = GetPiece(f);
            try
            {
                if (p.Equals(null)) return new List<Move>();
            }
            catch
            {
                return new List<Move>();
            }
            if (ActivePlayer().Color != p.Color&&checkChecking==false) return new List<Move>();
            List<Move> AvailableMoves = new List<Move>();
            Field newField;

            Field temp = GetKingPos(p.Color);
            if (checkChecking == false)
            {
                if (Covered(GetKingPos(p.Color))) ActivePlayer().InCheck = true;
                else ActivePlayer().InCheck = false;
            }
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
                        if (p.Field.Y == 2 && !Chessboard.Exists(x => x.Field.Y - 2 == p.Field.Y&& x.Field.X==p.Field.X) && AvailableMoves.Count != 0)
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
                        if (p.Field.Y == 5)
                        {
                            Piece foundPawn = Chessboard.Find(x => x.Field.Y == 5 && (x.Field.X == p.Field.X - 1 || x.Field.X == p.Field.X + 1) && x.Color != p.Color && x == LastMovedPiece && x.Name == "");
                            if (!foundPawn.Equals(null))
                            {
                                newField = new Field(foundPawn.Field.X, foundPawn.Field.Y + 1);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                EnPasantPosible = true;
                            }
                            else EnPasantPosible = false;
                        }
                        else EnPasantPosible = false;
                    }
                    else //black
                    {
                        if (!Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField)); //move 1
                        }
                        if (p.Field.Y == 7 && !Chessboard.Exists(x => x.Field.Y + 2 == p.Field.Y && x.Field.X == p.Field.X)&& AvailableMoves.Count!=0)
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
                        if (p.Field.Y == 4)
                        {
                            Piece foundPawn = Chessboard.Find(x => x.Field.Y == 4 && (x.Field.X == p.Field.X - 1 || x.Field.X == p.Field.X + 1) && x.Color != p.Color && x == LastMovedPiece && x.Name == "");
                            if (!foundPawn.Equals(null))
                            {
                                newField = new Field(foundPawn.Field.X, foundPawn.Field.Y - 1);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                EnPasantPosible = true;
                            }
                            else EnPasantPosible = false;
                        }
                        else EnPasantPosible = false;
                    }
                    break;
                case "K":
                    foreach (var a in Around(p))
                    {
                        if (Covered(a)) continue;
                        if (Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color != p.Color) && !Covered(a))
                        {
                            AvailableMoves.Add(new Move(p.Field, a));
                            //todo capture
                        }
                        else if ((Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color == p.Color))) continue;
                        AvailableMoves.Add(new Move(p.Field, a));
                    }
                    if (p.Moved == false&&ActivePlayer().InCheck==false)
                    {
                        CastleMoves = new List<Move>();
                        Move m = CheckCastleLong();
                        if (m != null)
                        {
                            CastleMoves.Add(m);
                            AvailableMoves.Add(m);
                        }
                        m = CheckCastleShort();
                        if (m != null)
                        {
                            CastleMoves.Add(m);
                            AvailableMoves.Add(m);
                        }
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
                        if (p.Field.X + i <= 8 && p.Field.Y + i <= 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//up right
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X - i > 0 && p.Field.Y + i <= 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//up left
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X - i > 0 && p.Field.Y - i > 0)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//down left
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i <= 8 && p.Field.Y - i > 0)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//down right
                    }//diagonals
                    break;
                case "B":
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i <= 8 && p.Field.Y + i <= 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//up right
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X - i > 0 && p.Field.Y + i <= 8)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//up left
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X - i > 0 && p.Field.Y - i > 0)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//down left
                    }
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i <= 8 && p.Field.Y - i > 0)
                        {
                            if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                                //todo capture
                            }
                            else if (Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
                            else
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                            }
                        }//down right
                    }
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
                        else if (Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X - 1 && x.Color == p.Color));
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
            if (ActivePlayer().InCheck == true&&checkChecking==false)
            {
                AvailableMoves=TestIfCheckResolved(AvailableMoves);
            }
            return AvailableMoves;
        }
        private bool LineEmptyAndNotCovered(bool color, string which)
        {
            Field f = new Field(GetKingPos(color).X,GetKingPos(color).Y);
            if (which == "right")
            {
                for(int i = 0; i < 2; i++)
                {
                    f.X++;
                    if (GetPiece(f) != null || Covered(f) == true) return false;
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    f.X--;
                    if (GetPiece(f) != null || Covered(f) == true) return false;
                }
            }
            return true;
        }
        private Move CheckCastleShort()
        {
            Field f = GetKingPos(ActivePlayer().Color);
            Move m = new Move(f, new Field(f.X + 2, f.Y));
            if (RookMoved(ActivePlayer().Color, "right"))
            {
                return null;
            }
            if (LineEmptyAndNotCovered(ActivePlayer().Color, "right"))
            {
                return m;
            }
            return null;
        }
        private Move CheckCastleLong()
        {
            Field f = GetKingPos(ActivePlayer().Color);
            Move m = new Move(f,new Field(f.X-2,f.Y));
            if (RookMoved(ActivePlayer().Color, "left"))
            {
                return null;
            }
            if (LineEmptyAndNotCovered(ActivePlayer().Color, "left"))
            {
                return m;
            }
            return null;
        }
        private bool RookMoved(bool color,string which)
        {
            Field f;
            if (color == true) f = new Field(1, 1);
            else f = new Field(1, 8);
            if (which == "right") f.X = 8;
            else f.X = 1;
            Piece p = GetPiece(f);
            if (p == null || p.Moved == true) return true;
            return false;
        }
        private List<Move> TestIfCheckResolved(List<Move> moves)
        {
            bool color = ActivePlayer().Color;
            List<Move> AvailableMoves = new List<Move>();
            List<Field> tempCoveredFields;
            checkChecking = true;
            foreach (var a in moves)
            { 
                ActivePlayer().InCheck = true;
                MockMove(a);
                tempCoveredFields = GetNewCoveredFields(color);
                Field f = GetKingPos(color);
                if (!tempCoveredFields.Exists(x => x.IsSame(f)))
                {
                    AvailableMoves.Add(a);
                }
                MockUnMove(a);
            }
            checkChecking = false;
            return AvailableMoves;
        }
        private void MockMove(Move m)
        {
            if (Chessboard.Exists(x => x.Field.IsSame(m.To)))
            {
                tempPiece = new Piece(GetPiece(m.To));
                Chessboard.Remove(Chessboard.Find(x => x==GetPiece(m.To))); //capture
            }
            GetPiece(m.From).Field = new Field(m.To.X, m.To.Y);
        }
        private void MockUnMove(Move m)
        {
            if (tempPiece != null) AddToChessBoard(tempPiece);
            GetPiece(m.To).Field = new Field(m.From.X, m.From.Y);
            tempPiece = null;
        }
        private void MoveRook(Move m)
        {
            if (m.To.X == 7 && m.From.Y == 1) GetPiece(new Field(8, 1)).Field = new Field(6, 1);
            if (m.To.X == 3 && m.From.Y == 1) GetPiece(new Field(1, 1)).Field = new Field(4, 1);
            if (m.To.X == 7 && m.From.Y == 8) GetPiece(new Field(8, 8)).Field = new Field(6, 8);
            if (m.To.X == 3 && m.From.Y == 8) GetPiece(new Field(1, 8)).Field = new Field(4, 8);
        }
        private void EnPasant(Move m)
        {
            GetPiece(m.From).Field = m.To;
            Chessboard.Remove(Chessboard.Find(x => x.Field.Y == m.From.Y && x.Field.X == m.To.X));
            EnPasantPosible = false;
            EnPasantHappened = true;
        }
        private void MakeMove(Move m)
        {
            Piece p = GetPiece(m.From);
            LastMovedPiece = p;
            if (p.Name == "" && (m.To.Y == 1 || m.To.Y == 8)) CheckPromotion = true;
            if (p.Name == "K") p.Moved = true;
            if (p.Name == "R") p.Moved = true;
            if (EnPasantPosible&&LastMovedPiece.Name=="") {
                if (p.Name == "" && m.To.Y != m.From.X&&m.To.X!=m.From.Y&&!Chessboard.Exists(x=>x.Field.X==m.To.X&&x.Field.Y==m.To.Y)) EnPasant(m) ;
             }
            //if (ActivePlayer().Color != Chessboard.Find(x => x.Field.X == m.From.X&&x.Field.Y==m.From.Y).Color) return; //todo premoves
            if (CastleMoves.Exists(x => x.IsSame(m))) MoveRook(m);
            if (Chessboard.Exists(x => x.Field.X == m.To.X && x.Field.Y == m.To.Y&&x.Color!=ActivePlayer().Color))
            {
                Chessboard.Remove(Chessboard.Find(x => x.Field.X == m.To.X && x.Field.Y == m.To.Y)); //capture
            }
            if(CheckPromotion==true) MovePromotion(m);
            p.Field = new Field(m.To.X, m.To.Y);
            CoveredFields = GetNewCoveredFields(!ActivePlayer().Color);
            if(Covered(GetKingPos(ActivePlayer().Color))) ActivePlayer().InCheck = false;
            ChangeTurn();
            MovesDone.Add(m);
            if (MovesPossible() == false) // game end
            {
                if (ActivePlayer().InCheck == true) result = (InactivePlayer().Color ? "white" : "black") + " wins";
                else result = "draw";
                return;
            }
        }
        public bool IsValid(Move m)
        {
            if (!Chessboard.Exists(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y)) return false; //validates if piece exists at desired from field
            if (!GetAvailableMoves(Chessboard.Find(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y).Field).Exists(x => x.To.X == m.To.X && x.To.Y == m.To.Y)) return false; //validates if move is possible
            MakeMove(m);
            return true;
        }
        private Piece GetPiece(Field f)
        {
            if (Chessboard.Exists(x => x.Field.X == f.X && x.Field.Y == f.Y)) return Chessboard.Find(x => x.Field.X == f.X && x.Field.Y == f.Y);
            else return null;
        }
        public GamestateChange GetFieldUpdates(Move m)
        {
            GamestateChange ret = new GamestateChange
            {
                UpdateFields = new List<FieldUpdate>(),
                GameResult = result
            };

            //if (m == null) // let engine do the move
            //{
            //    if (isComputerOpponent)
            //    {
            //        string engineMoveString = engineCommunicator.NextMove(BoardStateToFEN());
            //        Move engineMove = new Move(new Field(engineMoveString[0] - 97 + 1, engineMoveString[1] - '0'), new Field(engineMoveString[2] - 97 + 1, engineMoveString[3] - '0'));
            //        isComputerOpponent = false; // to not have computer play with itself
            //        MakeMove(engineMove);
            //        isComputerOpponent = true;
            //        return GetFieldUpdates(engineMove);
            //    }
            //    return ret;
            //}

            if (CastleMoves.Exists(x => x.IsSame(m)))
            {
                ret.UpdateFields.Add(new FieldUpdate(m.From));
                ret.UpdateFields.Add(new FieldUpdate(GetPiece(m.To)));
                if (m.To.X == 7 && m.To.Y == 1)
                {
                    ret.UpdateFields.Add(new FieldUpdate(new Field(8, 1)));
                    ret.UpdateFields.Add(new FieldUpdate(GetPiece(new Field(6, 1))));
                }
                else if (m.To.X == 3 && m.To.Y == 1)
                {
                    ret.UpdateFields.Add(new FieldUpdate(new Field(1, 1)));
                    ret.UpdateFields.Add(new FieldUpdate(GetPiece(new Field(4, 1))));
                }
                else if (m.To.X == 3 && m.To.Y == 8)
                {
                    ret.UpdateFields.Add(new FieldUpdate(new Field(1, 8)));
                    ret.UpdateFields.Add(new FieldUpdate(GetPiece(new Field(4, 8))));
                }
                else if (m.To.X == 7 && m.To.Y == 8)
                {
                    ret.UpdateFields.Add(new FieldUpdate(new Field(8, 8)));
                    ret.UpdateFields.Add(new FieldUpdate(GetPiece(new Field(6, 8))));
                }
            }
            else if (EnPasantHappened)
            {
                List<FieldUpdate> fields = new List<FieldUpdate>();
                fields.Add(new FieldUpdate(m.From));
                fields.Add(new FieldUpdate(GetPiece(m.To)));
                fields.Add(new FieldUpdate(new Field(m.To.X, m.From.Y)));
                EnPasantHappened = false;
                return new GamestateChange
                {
                    UpdateFields = fields,
                    GameResult = result
                };
            }
            else
            {
                ret.UpdateFields.Add(new FieldUpdate(m.From));
                ret.UpdateFields.Add(new FieldUpdate(GetPiece(m.To)));
            }

            return ret;
        }

        public string ListToString(List<Move> l)
        {
            string s = "";
            foreach (var a in l)
            {
                s += a.To.ToString() + " ";
            }
            return s;
        }

        private void ChangeTurn()
        {
            foreach (var a in Players) a.Turn = !a.Turn;
        }

        public List<FieldUpdate> LoadBoardstate(int id)
        {
            if (id == 0) // new game
            {
                SetStartingPosition();
                isComputerOpponent = false;
                return Chessboard.Select(x => new FieldUpdate(x)).ToList();
            }
            else
            {
                if (id <= puzzles.Count)
                {
                    SetCustomPositionFEN(puzzles[id-1]);
                    isComputerOpponent = true;
                    List<FieldUpdate> ret = Chessboard.Select(x => new FieldUpdate(x)).ToList();
                    return ret;
                }
            }
            return null;
        }
    }
}
 