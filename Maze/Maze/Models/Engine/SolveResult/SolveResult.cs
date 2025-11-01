using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Models;
using Maze.Engine.Models;

namespace Maze.Models.Engine
{
    public class SolveResult
    {
        public SolveResult(int numberOfMoves, int timeTakenInMilliseconds, bool solved, PlayerNumber playerNumber)
        {
            this.NumberOfMoves = numberOfMoves;
            this.Solved = solved;
            this.PlayerNumber = playerNumber;
        }

        public int NumberOfMoves { get; private set; }
        public bool Solved { get; private set; }
        public PlayerNumber PlayerNumber { get; private set; }
    }
}
