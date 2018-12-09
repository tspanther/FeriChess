using FeriChess.Interfaces;
using FeriChess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FeriChess.Services
{
    public class MockBoardService : IBoardService
    {
        public List<Move> GetAvailableMoves(Field f)
        {
            var ret = new List<Move>
            {
                new Move(new Field(1, 2), new Field(2, 2))
            };
            return ret;
        }

        public List<FieldUpdate> GetFieldUpdates(Move m)
        {
            var ret = new List<FieldUpdate>
            {
                new FieldUpdate(new Field(1, 2))
            };
            return ret;
        }

        public bool IsValid(Move m)
        {
            return true;
        }

        public string ListToString(List<Move> l)
        {
            return "mock";
        }

        public List<FieldUpdate> LoadBoardstate()
        {
            var ret = new List<FieldUpdate>
            {
                new FieldUpdate(new Field(1, 2))
            };
            return ret;
        }

        public void SetStartingPosition()
        {
            return;
        }
    }
}