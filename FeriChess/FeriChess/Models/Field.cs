using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class Field
    {
        public Field(int x, int y)
        {
            try
            {
                if (x > 8 || x < 1 || y > 8 || y < 1) throw (new Exception());
                X = x;
                Y = y;
            }
            catch(Exception)
            {
                Console.WriteLine("Invalid placement"); //todo
            }
        }
        public bool IsSame(Field f)
        {
            if (f.X == this.X && f.Y == this.Y) return true;
            return false;
        }
        public int X { get; set; }
        public int Y { get; set; }
        public override string ToString()
        {
            string s = "";
            s += (char)(X + 96);
            s += Y;
            return s;
        }
    }
}