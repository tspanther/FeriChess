using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class BoardState
    {
        public Piece LastMovedPiece { get; set; }
        public bool EnPasantHappened { get; set; } // = false;
        public bool EnPasantPosible { get; set; } //= false;
        public bool CheckPromotion { get; set; } //= false;
        public bool CheckChecking { get; set; } //= false;
        public Piece TempPiece { get; set; } //= null;
        public List<Player> Players { get; set; } //= new List<Player>();
        public List<Field> CoveredFields { get; set; } //= new List<Field>();
        public List<Piece> Chessboard { get; set; } //= new List<Piece>();
        public string Result { get; set; } //= "";
        public List<Move> CastleMoves { get; set; } //= new List<Move>();
        public List<Move> MovesDone { get; set; } //= new List<Move>();
    }
}