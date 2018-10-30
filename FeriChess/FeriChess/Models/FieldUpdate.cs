using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    /// <summary>
    /// Class for returning changes for updating chessboard on the client.
    /// Field: Field
    /// Specifies which field needs to be updated
    /// PopulateBy: string
    /// Specifies the piece that shall populate the field. null if none (the field should become empty)
    /// </summary>
    public class FieldUpdate
    {
        public Field Field { get; set; }
        public string PopulateBy { get; set; }

        /// <summary>
        /// Constructs a FieldUpdate from a Piece.
        /// Sets Field to _piece.Field and PopulateBy to string, combined of: first letter of color + first letter of chess piece
        /// </summary>
        /// <param name="_piece"></param>
        public FieldUpdate(Piece _piece)
        {
            Field = _piece.Field;
            PopulateBy = (_piece.Color ? "w" : "b") + (_piece.Name.Length == 0 ? _piece.Name[0] : 'p');
        }

        /// <summary>
        /// Constructs a FieldUpdate from a Field.
        /// Sets Field to _emptyField and PopulateBy to null.
        /// Used for setting a field to empty
        /// </summary>
        /// <param name="field"></param>
        /// <param name=""></param>
        public FieldUpdate(Field _emptyField)
        {
            Field = _emptyField;
            PopulateBy = null;
        }
    }
}