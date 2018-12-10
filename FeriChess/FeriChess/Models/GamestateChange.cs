using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Models
{
    public class GamestateChange
    {
        public List<FieldUpdate> UpdateFields { get; set; }
        public string GameResult { get; set; }
    }
}