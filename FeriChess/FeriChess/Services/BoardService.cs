using FeriChess.Models;
using FeriChess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using FeriChess.EngineCommunicator;
using System.Text;
using FeriChess.Services.Interfaces;

namespace FeriChess.Services
{
    public class BoardService : IBoardService
    {
        private static IEngineCommunicator engineCommunicator;
        private static IFENService fENService;
        private static PuzzleService puzzleService;

        private BoardState boardState { get; set; }
        public bool isComputerOpponent { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_engineCommunicator"></param>
        /// <param name="_fENService"></param>
        public BoardService(
            IEngineCommunicator _engineCommunicator,
            IFENService _fENService
            )
        {
            boardState = new BoardState()
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
            engineCommunicator = _engineCommunicator;
            fENService = _fENService;
            puzzleService = new PuzzleService();
            isComputerOpponent = false;
            SetStartingPosition();
        }

        #region Public methods

        /// <summary>
        /// Sets boardState.Chessboard to classical starting position
        /// </summary>
        public void SetStartingPosition()
        {
            boardState.Chessboard = new List<Piece>();
            for (int i = 1; i < 9; i++)
            {
                boardState.Chessboard.Add(new Piece(new Field(i, 2), true, ""));
            }
            boardState.Chessboard.Add(new Piece(new Field(1, 1), true, "R"));
            boardState.Chessboard.Add(new Piece(new Field(8, 1), true, "R"));
            boardState.Chessboard.Add(new Piece(new Field(2, 1), true, "N"));
            boardState.Chessboard.Add(new Piece(new Field(7, 1), true, "N"));
            boardState.Chessboard.Add(new Piece(new Field(3, 1), true, "B"));
            boardState.Chessboard.Add(new Piece(new Field(6, 1), true, "B"));
            boardState.Chessboard.Add(new Piece(new Field(4, 1), true, "Q"));
            boardState.Chessboard.Add(new Piece(new Field(5, 1), true, "K"));
            for (int i = 1; i < 9; i++)
            {
                boardState.Chessboard.Add(new Piece(new Field(i, 7), false, ""));
            }
            boardState.Chessboard.Add(new Piece(new Field(1, 8), false, "R"));
            boardState.Chessboard.Add(new Piece(new Field(8, 8), false, "R"));
            boardState.Chessboard.Add(new Piece(new Field(2, 8), false, "N"));
            boardState.Chessboard.Add(new Piece(new Field(7, 8), false, "N"));
            boardState.Chessboard.Add(new Piece(new Field(3, 8), false, "B"));
            boardState.Chessboard.Add(new Piece(new Field(6, 8), false, "B"));
            boardState.Chessboard.Add(new Piece(new Field(4, 8), false, "Q"));
            boardState.Chessboard.Add(new Piece(new Field(5, 8), false, "K"));
        }

        /// <summary>
        /// Returns list of possible moves from the certain Field based on current boardState
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public List<Move> GetAvailableMoves(Field f)
        {
            Piece p = GetPiece(f);
            if (p == null) return new List<Move>();
            if (ActivePlayer().Color != p.Color && boardState.CheckChecking == false) return new List<Move>();
            List<Move> AvailableMoves = new List<Move>();
            Field newField;

            Field temp = GetKingPos(p.Color);
            if (boardState.CheckChecking == false)
            {
                if (Covered(GetKingPos(p.Color))) ActivePlayer().InCheck = true;
                else ActivePlayer().InCheck = false;
            }
            switch (p.Name)
            {
                case "":
                    if (p.Color)//white
                    {
                        if (!boardState.Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField)); //move 1
                        }
                        if (p.Field.Y == 2 && !boardState.Chessboard.Exists(x => x.Field.Y - 2 == p.Field.Y && x.Field.X == p.Field.X) && AvailableMoves.Count != 0)
                        {
                            newField = new Field(p.Field.X, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//move 2
                        if (boardState.Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X - 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//capture
                        if (boardState.Chessboard.Exists(x => x.Field.Y - 1 == p.Field.Y && x.Field.X + 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//capture
                        if (p.Field.Y == 5)
                        {
                            Piece foundPawn = boardState.Chessboard
                                .Find(x => x.Field.Y == 5
                            && (x.Field.X == p.Field.X - 1 || x.Field.X == p.Field.X + 1)
                            && x.Color != p.Color && x == boardState.LastMovedPiece && x.Name == "");

                            if (foundPawn != null)
                            {
                                newField = new Field(foundPawn.Field.X, foundPawn.Field.Y + 1);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                boardState.EnPasantPosible = true;
                            }
                            else boardState.EnPasantPosible = false;
                        }
                        else boardState.EnPasantPosible = false;
                    }
                    else //black
                    {
                        if (!boardState.Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X == p.Field.X))
                        {
                            newField = new Field(p.Field.X, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField)); //move 1
                        }
                        if (p.Field.Y == 7 && !boardState.Chessboard.Exists(x => x.Field.Y + 2 == p.Field.Y && x.Field.X == p.Field.X) && AvailableMoves.Count != 0)
                        {
                            newField = new Field(p.Field.X, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }//move 2
                        if (boardState.Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X - 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        if (boardState.Chessboard.Exists(x => x.Field.Y + 1 == p.Field.Y && x.Field.X + 1 == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        if (p.Field.Y == 4)
                        {
                            Piece foundPawn = boardState.Chessboard
                                .Find(x => x.Field.Y == 4
                            && (x.Field.X == p.Field.X - 1 || x.Field.X == p.Field.X + 1)
                            && x.Color != p.Color && x == boardState.LastMovedPiece && x.Name == "");

                            if (foundPawn != null)
                            {
                                newField = new Field(foundPawn.Field.X, foundPawn.Field.Y - 1);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                boardState.EnPasantPosible = true;
                            }
                            else boardState.EnPasantPosible = false;
                        }
                        else boardState.EnPasantPosible = false;
                    }
                    break;
                case "K":
                    foreach (var a in Around(p))
                    {
                        if (Covered(a)) continue;
                        if (boardState.Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color != p.Color) && !Covered(a))
                        {
                            AvailableMoves.Add(new Move(p.Field, a));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.X == a.X && x.Field.Y == a.Y && x.Color == p.Color)) continue;
                        AvailableMoves.Add(new Move(p.Field, a));
                    }
                    if (p.Moved == false && ActivePlayer().InCheck == false)
                    {
                        boardState.CastleMoves = new List<Move>();
                        Move m = CheckCastleLong();
                        if (m != null)
                        {
                            boardState.CastleMoves.Add(m);
                            AvailableMoves.Add(m);
                        }
                        m = CheckCastleShort();
                        if (m != null)
                        {
                            boardState.CastleMoves.Add(m);
                            AvailableMoves.Add(m);
                        }
                    }
                    break;
                case "Q":
                    for (int i = p.Field.Y + 1; i <= 8; i++)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//up
                    for (int i = p.Field.Y - 1; i > 0; i--)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//down
                    for (int i = p.Field.X - 1; i > 0; i--)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//left
                    for (int i = p.Field.X + 1; i <= 8; i++)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//right
                    for (int i = 1; i <= 8; i++)
                    {
                        if (p.Field.X + i <= 8 && p.Field.Y + i <= 8)
                        {
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y + i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y + i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X - i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X - i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
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
                            if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color != p.Color))
                            {
                                newField = new Field(p.Field.X + i, p.Field.Y - i);
                                AvailableMoves.Add(new Move(p.Field, newField));
                                break;
                            }
                            else if (boardState.Chessboard.Exists(x => x.Field.X == p.Field.X + i && x.Field.Y == p.Field.Y - i && x.Color == p.Color)) break;
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
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X + 2 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y + 2 <= 8 && p.Field.X + 1 <= 8)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X + 1 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 2 > 0 && p.Field.X + 1 <= 8)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X + 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X + 1 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X + 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.X - 2 > 0 && p.Field.Y + 1 <= 8)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 1 && x.Field.X == p.Field.X - 2 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y + 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.X + 2 <= 8 && p.Field.Y - 1 > 0)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X + 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X + 2 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X + 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y + 2 <= 8 && p.Field.X - 1 > 0)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y + 2 && x.Field.X == p.Field.X - 1 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y + 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 1 > 0 && p.Field.X - 2 > 0)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X - 2 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 1 && x.Field.X == p.Field.X - 2 && x.Color == p.Color)) ;
                        else
                        {
                            newField = new Field(p.Field.X - 2, p.Field.Y - 1);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                    }
                    if (p.Field.Y - 2 > 0 && p.Field.X - 1 > 0)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X - 1 && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X - 1, p.Field.Y - 2);
                            AvailableMoves.Add(new Move(p.Field, newField));
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == p.Field.Y - 2 && x.Field.X == p.Field.X - 1 && x.Color == p.Color)) ;
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
                        if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//up
                    for (int i = p.Field.Y - 1; i > 0; i--)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color != p.Color))
                        {
                            newField = new Field(p.Field.X, i);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.Y == i && x.Field.X == p.Field.X && x.Color == p.Color)) break;
                        newField = new Field(p.Field.X, i);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//down
                    for (int i = p.Field.X - 1; i > 0; i--)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//left
                    for (int i = p.Field.X + 1; i <= 8; i++)
                    {
                        if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color != p.Color))
                        {
                            newField = new Field(i, p.Field.Y);
                            AvailableMoves.Add(new Move(p.Field, newField));
                            break;
                        }
                        else if (boardState.Chessboard.Exists(x => x.Field.X == i && x.Field.Y == p.Field.Y && x.Color == p.Color)) break;
                        newField = new Field(i, p.Field.Y);
                        AvailableMoves.Add(new Move(p.Field, newField));
                    }//right
                    break;
            }
            if (ActivePlayer().InCheck == true && boardState.CheckChecking == false)
            {
                AvailableMoves = TestIfCheckResolved(AvailableMoves);
            }
            return AvailableMoves;
        }

        /// <summary>
        /// Returns the list of fields that need updating after a certain Move
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public GamestateChange GetFieldUpdates(Move m)
        {
            GamestateChange ret = new GamestateChange
            {
                UpdateFields = new List<FieldUpdate>(),
                GameResult = boardState.Result
            };

            if (boardState.CastleMoves.Exists(x => x.IsSame(m)))
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
            else if (boardState.EnPasantHappened)
            {
                List<FieldUpdate> fields = new List<FieldUpdate>();
                fields.Add(new FieldUpdate(m.From));
                fields.Add(new FieldUpdate(GetPiece(m.To)));
                fields.Add(new FieldUpdate(new Field(m.To.X, m.From.Y)));
                boardState.EnPasantHappened = false;
                return new GamestateChange
                {
                    UpdateFields = fields,
                    GameResult = boardState.Result
                };
            }
            else
            {
                ret.UpdateFields.Add(new FieldUpdate(m.From));
                ret.UpdateFields.Add(new FieldUpdate(GetPiece(m.To)));
            }

            return ret;
        }

        /// <summary>
        /// Validates the move
        /// </summary>
        /// <param name="m"></param>
        /// <returns>true, is the move is valid, false if invalid</returns>
        public bool IsValid(Move m)
        {
            if (!boardState.Chessboard.Exists(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y)) return false; //validates if piece exists at desired from field
            if (!GetAvailableMoves(boardState.Chessboard.Find(x => x.Field.X == m.From.X && x.Field.Y == m.From.Y).Field).Exists(x => x.To.X == m.To.X && x.To.Y == m.To.Y)) return false; //validates if move is possible
            return true;
        }

        /// <summary>
        /// Loads a boardState corresponding to certain ID;
        /// Updates boardState to state fetched by ID
        /// Returns List of occupied fields
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of occupied fields</returns>
        public List<FieldUpdate> LoadBoardstate(int id)
        {
            if (id == 0) // new game
            {
                Reset();
                SetStartingPosition();
                isComputerOpponent = false;
                return boardState.Chessboard.Select(x => new FieldUpdate(x)).ToList();
            }
            else
            {
                if (id <= puzzleService.puzzles.Count)
                {
                    boardState = fENService.SetCustomPositionFEN(puzzleService.puzzles[id - 1]);
                    isComputerOpponent = true;
                    List<FieldUpdate> ret = boardState.Chessboard.Select(x => new FieldUpdate(x)).ToList();
                    return ret;
                }
            }
            return null;
        }

        /// <summary>
        /// Makes a move on the boardstate
        /// </summary>
        /// <param name="move"></param>
        /// <returns>Change of game's state in GamestateChange object. No change if invalid move</returns>
        public GamestateChange MakeMove(Move move)
        {
            if (!IsValid(move))
            {
                return new GamestateChange
                {
                    UpdateFields = new List<FieldUpdate>(),
                    GameResult = ""
                };
            }

            Piece p = GetPiece(move.From);
            boardState.LastMovedPiece = p;
            if (p.Name == "" && (move.To.Y == 1 || move.To.Y == 8)) boardState.CheckPromotion = true;
            if (p.Name == "K") p.Moved = true;
            if (p.Name == "R") p.Moved = true;
            if (boardState.EnPasantPosible && boardState.LastMovedPiece.Name == "")
            {
                if (p.Name == "" && move.To.Y != move.From.X && move.To.X != move.From.Y && !boardState.Chessboard.Exists(x => x.Field.X == move.To.X && x.Field.Y == move.To.Y)) EnPasant(move);
            }
            if (boardState.CastleMoves.Exists(x => x.IsSame(move))) MoveRook(move);
            if (boardState.Chessboard.Exists(x => x.Field.X == move.To.X && x.Field.Y == move.To.Y && x.Color != ActivePlayer().Color))
            {
                boardState.Chessboard.Remove(boardState.Chessboard.Find(x => x.Field.X == move.To.X && x.Field.Y == move.To.Y)); //capture
            }
            if (boardState.CheckPromotion == true) MovePromotion(move);
            p.Field = new Field(move.To.X, move.To.Y);
            boardState.CoveredFields = GetNewCoveredFields(!ActivePlayer().Color);
            if (Covered(GetKingPos(ActivePlayer().Color))) ActivePlayer().InCheck = false;
            ChangeTurn();
            if (MovesPossible() == false)
            {
                if (ActivePlayer().InCheck == true) boardState.Result = (InactivePlayer().Color ? "white" : "black") + " wins";
                else boardState.Result = "draw";
            }
            boardState.MovesDone.Add(move);
            if (ThreeFoldRep()) boardState.Result = "draw";
            if (InsufficientMaterial()) boardState.Result = "draw";

            return GetFieldUpdates(move);
        }

        /// <summary>
        /// Asks engine for a move, makes it and returns change of game state
        /// </summary>
        /// <returns>GamestateChange after engine move</returns>
        public GamestateChange RequestEngineMove()
        {
            string fen = fENService.BoardStateToFEN(boardState);
            string engineMoveString = engineCommunicator.NextMove(fen);
            Move engineMove = new Move(new Field(engineMoveString[0] - 97 + 1, engineMoveString[1] - '0'), new Field(engineMoveString[2] - 97 + 1, engineMoveString[3] - '0'));
            return MakeMove(engineMove);
        }

        #endregion

        #region Kekec
        /// <summary>
        /// Sets Chessboard from string generated by GameStateToString
        /// </summary>
        /// <param name="s"></param>
        public void SetCustomPosition(string s)
        {
            boardState.Chessboard = new List<Piece>();
            string[] pieces = s.Split(',');
            foreach (var a in pieces)
            {
                boardState.Chessboard.Add(new Piece(new Field(a[2], a[3] - '0'), a[0] == 1 ? true : false, a[1].ToString()));
            }
        }
        /// <summary>
        /// Converts all executed moves to string
        /// </summary>
        /// <returns></returns>
        public string MovesToString()
        {
            string s="";
            foreach(Move a in boardState.MovesDone)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        /// <summary>
        /// Converts chessboard to string
        /// </summary>
        /// <returns></returns>
        public string GameStateToString()
        {
            string s="";
            foreach(var a in boardState.Chessboard)
            {
                s += a.ToString() + ',';
            }
            return s;
        }
        /// <summary>
        /// Promotes a pawn on 8th rank to queen
        /// </summary>
        /// <param name="m"></param>
        private void MovePromotion(Move m)
        {
            string Choice = "Q";
            boardState.Chessboard.Add(new Piece(new Field(m.To.X, m.To.Y), ActivePlayer().Color, Choice));
            boardState.Chessboard.Remove(GetPiece(m.From));
            boardState.CheckPromotion = false;

        }
        /// <summary>
        /// returns list of fields attacked by enemy pieces
        /// </summary>
        /// <param name="Color"></param>
        /// <returns></returns>
        private List<Field> GetNewCoveredFields(bool Color)
        {
            List<Field> CoveredFields = new List<Field>();
            List<Move> temp;
            foreach (var a in boardState.Chessboard.ToList())
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
                            if (!CoveredFields.Exists(x => x == b.To))
                            {
                                CoveredFields.Add(b.To);
                            }
                        }
                    }
                }
            }
            return CoveredFields;
        }
        /// <summary>
        /// returns Chessboard
        /// </summary>
        /// <returns></returns>
        private List<Piece> GetChessBoard()
        {
            return boardState.Chessboard;
        }
        /// <summary>
        /// adds piece to chessboard
        /// </summary>
        /// <param name="p"></param>
        private void AddToChessBoard(Piece p)
        {
            boardState.Chessboard.Add(p);
        }
        /// <summary>
        /// same as BoardStateToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            foreach (var a in boardState.Chessboard)
            {
                s += a.ToString() + " ";
            }
            return s;
        }
        /// <summary>
        /// returns fields attacked by pawn
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
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
        /// <summary>
        /// returns all fields around piece, used for king movement
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
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
        /// <summary>
        /// checks if field is covered
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private bool Covered(Field f)
        {
            if (boardState.CoveredFields.Exists(x => x.X == f.X && x.Y == f.Y)) return true;
            return false;
        }
        /// <summary>
        /// returns active player
        /// </summary>
        /// <returns></returns>
        private Player ActivePlayer()
        {
            return boardState.Players.Find(x => x.Turn == true);
        }
        /// <summary>
        /// returns inactive player
        /// </summary>
        /// <returns></returns>
        private Player InactivePlayer()
        {
            return boardState.Players.Find(x => x.Turn == false);
        }
        /// <summary>
        /// returns king's position
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private Field GetKingPos(bool color)
        {
            return boardState.Chessboard.Find(x => x.Name == "K" && x.Color == color).Field;
        }
        /// <summary>
        /// checks if any moves are possible
        /// </summary>
        /// <returns></returns>
        private bool MovesPossible()
        {
            foreach (var a in boardState.Chessboard.ToList())
            {
                if (a.Color != ActivePlayer().Color) continue;
                if (GetAvailableMoves(a.Field).Count != 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// checks if line between king is not occupied or attacked by enemy pieces
        /// </summary>
        /// <param name="color"></param>
        /// <param name="which"></param>
        /// <returns></returns>
        private bool LineEmptyAndNotCovered(bool color, string which)
        {
            Field f = new Field(GetKingPos(color).X, GetKingPos(color).Y);
            if (which == "right")
            {
                for (int i = 0; i < 2; i++)
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
        /// <summary>
        /// checks if short castle is possible
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// checks if long castle is possible
        /// </summary>
        /// <returns></returns>
        private Move CheckCastleLong()
        {
            Field f = GetKingPos(ActivePlayer().Color);
            Move m = new Move(f, new Field(f.X - 2, f.Y));
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
        /// <summary>
        /// checks if rook has been moved
        /// </summary>
        /// <param name="color"></param>
        /// <param name="which"></param>
        /// <returns></returns>
        private bool RookMoved(bool color, string which)
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
        /// <summary>
        /// tests available moves if they resolve check, returns updated list of available moves
        /// </summary>
        /// <param name="moves"></param>
        /// <returns></returns>
        private List<Move> TestIfCheckResolved(List<Move> moves)
        {
            bool color = ActivePlayer().Color;
            List<Move> AvailableMoves = new List<Move>();
            List<Field> tempCoveredFields;
            boardState.CheckChecking = true;
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
            boardState.CheckChecking = false;
            return AvailableMoves;
        }
        /// <summary>
        /// makes a mock move
        /// </summary>
        /// <param name="m"></param>
        private void MockMove(Move m)
        {
            if (boardState.Chessboard.Exists(x => x.Field.IsSame(m.To)))
            {
                boardState.TempPiece = new Piece(GetPiece(m.To));
                boardState.Chessboard.Remove(boardState.Chessboard.Find(x => x == GetPiece(m.To))); //capture
            }
            GetPiece(m.From).Field = new Field(m.To.X, m.To.Y);
        }
        /// <summary>
        /// undos the mock move
        /// </summary>
        /// <param name="m"></param>
        private void MockUnMove(Move m)
        {
            try
            {
                if (boardState.TempPiece != null) AddToChessBoard(boardState.TempPiece);
            }
            catch { }
            GetPiece(m.To).Field = new Field(m.From.X, m.From.Y);
            boardState.TempPiece = null;
        }
        /// <summary>
        /// moves the rook when castling
        /// </summary>
        /// <param name="m"></param>
        private void MoveRook(Move m)
        {
            if (m.To.X == 7 && m.From.Y == 1) GetPiece(new Field(8, 1)).Field = new Field(6, 1);
            if (m.To.X == 3 && m.From.Y == 1) GetPiece(new Field(1, 1)).Field = new Field(4, 1);
            if (m.To.X == 7 && m.From.Y == 8) GetPiece(new Field(8, 8)).Field = new Field(6, 8);
            if (m.To.X == 3 && m.From.Y == 8) GetPiece(new Field(1, 8)).Field = new Field(4, 8);
        }
        /// <summary>
        /// performs enpassant
        /// </summary>
        /// <param name="m"></param>
        private void EnPasant(Move m)
        {
            GetPiece(m.From).Field = m.To;
            boardState.Chessboard.Remove(boardState.Chessboard.Find(x => x.Field.Y == m.From.Y && x.Field.X == m.To.X));
            boardState.EnPasantPosible = false;
            boardState.EnPasantHappened = true;
        }
        /// <summary>
        /// checks if sufficient material remains on the board
        /// </summary>
        /// <returns></returns>
        private bool InsufficientMaterial()
        {
            if (boardState.Chessboard.Count < 4)
            {
                if (boardState.Chessboard.Count(x => x.Name == "" || x.Name == "Q" || x.Name == "R") == 0)
                {
                    if (boardState.Chessboard.Count <= 3) return true;
                    if (boardState.Chessboard.Count(x => x.Name == "B" && x.Color == true) == 1 && boardState.Chessboard.Count(x => x.Name == "B" && x.Color == false) == 1) return true;
                }
            }
            return false;
        }
        /// <summary>
        /// checks for threefold repetition
        /// </summary>
        /// <returns></returns>
        private bool ThreeFoldRep()
        {
            int moveNum = boardState.MovesDone.Count();
            if (moveNum < 6) return false;
            if (boardState.MovesDone[moveNum - 1].To.IsSame(boardState.MovesDone[moveNum - 5].To) 
                && boardState.MovesDone[moveNum - 3].To.IsSame(boardState.MovesDone[moveNum - 7].To) 
                && boardState.MovesDone[moveNum - 6].To.IsSame(boardState.MovesDone[moveNum - 2].To) 
                && boardState.MovesDone[moveNum - 8].To.IsSame(boardState.MovesDone[moveNum - 4].To)) return true;
            return false;
        }
        /// <summary>
        /// returns the piece occupying field f
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        private Piece GetPiece(Field f)
        {
            if (boardState.Chessboard.Exists(x => x.Field.X == f.X && x.Field.Y == f.Y)) return boardState.Chessboard.Find(x => x.Field.X == f.X && x.Field.Y == f.Y);
            else return null;
        }
        /// <summary>
        /// returns a string of moves
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        public string ListToString(List<Move> l)
        {
            string s = "";
            foreach (var a in l)
            {
                s += a.To.ToString() + " ";
            }
            return s;
        }
        /// <summary>
        /// switches turn
        /// </summary>
        private void ChangeTurn()
        {
            foreach (var a in boardState.Players) a.Turn = !a.Turn;
        }
        private void Reset()
        {
            boardState = new BoardState()
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
        }
    }
        #endregion
}
 