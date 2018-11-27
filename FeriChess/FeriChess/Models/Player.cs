using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class Player
    {
        public string Username { set; get; }
        public bool Turn { set; get; }
        public bool Color { set; get; }
        public bool InCheck { set; get; }
        public double Time { set; get; }
        public int Increment { set; get; }
        public Player(string username, bool color, double time, int increment)
        {
            Username = username;
            Color = color;
            if (Color == true) Turn = true;
            Time = time;
            Increment = increment;
            InCheck = false;
        }
        public override string ToString()
        {
            return Username+" "+Color+" "+Time+" "+Increment;
        }
    }
}