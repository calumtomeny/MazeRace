using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models.Engine;

namespace Maze.Engine.Models
{
    public class CompleteSolveInfo
    {
        public CompleteSolveInfo(IEnumerable<SolveResult> solveResults, long timeTakenInMilliseconds, MazeSolveReplay replay, bool endGame)
        {
            this.solveResults = solveResults;
            this.TimeTakenInMilliseconds = timeTakenInMilliseconds;
            this.replay = replay;
            this.EndGame = endGame;
        }
        public IEnumerable<SolveResult> solveResults { get; private set; }
        public long TimeTakenInMilliseconds { get; private set; }
        public MazeSolveReplay replay { get; private set; }
        public bool EndGame { get; set; }
    }
}
