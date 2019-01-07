using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class Move
    {
        public Move(Field from, Field to)
        {
            From = from;
            To = to;
        }
        public Field From { get; set; }
        public Field To { get; set; }
        public bool IsSame(Move m)
        {
            if (From.IsSame(m.From) && To.IsSame(m.To)) return true;
            return false;
        }
        //public Move(string s)
        //{
        //    From = new Field(s[0]-'0', s[1]-'0');
        //    To = new Field(s[2] - '0', s[3] - '0');

        //}
        public override string ToString()
        {
            return From.ToString() + To.ToString();
        }
    }
}