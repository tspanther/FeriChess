﻿using FeriChess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FeriChess.Interfaces
{
    public interface IBoardService
    {
        /// <summary>
        /// Stores this
        /// </summary>
        //List<Piece> ChessBoard { get; set; }
        // ^ commented out because you still only have fields instead of properties in BoardService implementation

        bool isComputerOpponent { get; set; }

        /// <summary>
        /// Gets possible moves for the piece on the selected field
        /// </summary>
        /// <param name="f"></param>
        /// <returns>List of possible moves</returns>
        List<Move> GetAvailableMoves(Field f);

        List<FieldUpdate> LoadBoardstate(int id);

        /// <summary>
        /// Checks if move is valid and executes the move.
        /// </summary>
        /// <param name="m"></param>
        /// <returns>bool whether move was executed</returns>
        bool IsValid(Move m);

        /// <summary>
        /// Converts a list of moves to string of possible squares separated by spaces
        /// </summary>
        /// <param name="l"></param>
        /// <returns>string of possible fields separated by spaces</returns>
        string ListToString(List<Move> l);

        /// <summary>
        /// Converts the list of pieces to string
        /// </summary>
        /// <returns>string of format:{color}{name}{field} for each piece</returns>
        string ToString();

        /// <summary>
        /// Sets the chessboard back to it's default position
        /// </summary>
        void SetStartingPosition();

        /// <summary>
        /// Returns list of updated fields after making a move
        /// </summary>
        /// <param name="m"></param>
        /// <returns>list of updated fields after making a move</returns>
        GamestateChange GetFieldUpdates(Move m);

        /// <summary>
        /// Makes a move on the boardstate
        /// </summary>
        /// <param name="move"></param>
        /// <returns>Change of game's state in GamestateChange object. No change if invalid move</returns>
        GamestateChange MakeMove(Move move);

        /// <summary>
        /// Asks engine for a move, makes it and returns change of game state
        /// </summary>
        /// <returns>GamestateChange after engine move</returns>
        GamestateChange RequestEngineMove();
    }
}
