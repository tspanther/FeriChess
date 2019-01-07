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
        public bool isComputerOpponent { get; set; }

        public List<Move> GetAvailableMoves(Field f)
        {
            var ret = new List<Move>
            {
                new Move(new Field(1, 2), new Field(2, 2))
            };
            return ret;
        }

        public GamestateChange GetFieldUpdates(Move m)
        {
            var ret = new List<FieldUpdate>
            {
                new FieldUpdate(new Field(1, 2))
            };
            return new GamestateChange
            {
                UpdateFields = ret,
                GameResult = ""
            };
        }

        public bool IsValid(Move m)
        {
            return true;
        }

        public string ListToString(List<Move> l)
        {
            return "mock";
        }

        public List<FieldUpdate> LoadBoardstate(int id)
        {
            var ret = new List<FieldUpdate>
            {
                new FieldUpdate(new Field(1, 2))
            };
            return ret;
        }

        public GamestateChange MakeMove(Move m)
        {
            return new GamestateChange()
            {
                GameResult = "",
                UpdateFields = new List<FieldUpdate>() { new FieldUpdate(new Field(1, 2)) }
            };
        }

        public void SetStartingPosition()
        {
            return;
        }

        public GamestateChange RequestEngineMove()
        {
            return new GamestateChange();
        }
    }
}