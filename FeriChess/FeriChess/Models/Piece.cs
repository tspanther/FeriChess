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
        public bool Moved { get; set; }
        public Piece(Field f,bool c,string n)
        {
            Field = f;
            Color = c;
            Name = n;
            Moved = false;
        }
        public static bool operator == (Piece one,Piece two)
        {
            if (one.Field.X == two.Field.X && one.Field.Y == two.Field.Y) return true;
            return false;
        }
        public static bool operator !=(Piece one, Piece two)
        {
            if (one.Field.X == two.Field.X && one.Field.Y == two.Field.Y) return false;
            return true;
        }
        public Piece(Piece p)
        {
            Field = p.Field;
            Color = p.Color;
            Name = p.Name;
            Moved = p.Moved;
        }
        //public Piece(Piece p)
        //{
        //    Field = p.Field;
        //    Color = p.Color;
        //    Name = p.Name;
        //}
        public override string ToString()
        {
            string s = "";
            if (Color) s += "1";
            else s += "0";
            s += Name + Field.ToString();
            return s;
        }
    }
}