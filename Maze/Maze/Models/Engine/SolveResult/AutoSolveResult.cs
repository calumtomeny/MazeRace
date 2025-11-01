using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Models;
using Maze.Engine.Models;

namespace Maze.Models.Engine
{
    public class ComputerSolveResult : SolveResult
    {
        public ComputerSolveResult(int numberOfMoves, int timeTakenInMilliseconds, bool solved, PlayerNumber playerNumber) :
            base(numberOfMoves, timeTakenInMilliseconds, solved, playerNumber){
        }
    }
}
