using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class Piece
    {
        public Field Field { get; set; }
        public bool Color { get; set; }
        public string Name { get; set; }
        public Piece(Field f,bool c,string n)
        {
            Field = f;
            Color = c;
            Name = n;
        }
        public override string ToString()
        {
            string s = "";
            if (Color) s += "white";
            else s += "black";
            s += Name + Field.ToString();
            return s;
        }
    }
}