using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Models;
using Maze.Engine.Models;

namespace Maze.Models.Engine
{
    public class HumanSolveResult : SolveResult
    {
        public HumanSolveResult(int numberOfMoves, int timeTakenInMilliseconds, bool solved, int numberOfActions, PlayerNumber playerNumber) :
            base(numberOfMoves,timeTakenInMilliseconds,solved, playerNumber)
        {
            this.NumberOfActions = numberOfActions;
        }

        public int NumberOfActions { get; private set; }
    }
}
