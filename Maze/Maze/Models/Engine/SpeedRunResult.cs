using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Engine.Models;

namespace Maze.Models.Engine
{
    public class SpeedRunResult
    {
        public string PlayerName { get; set; }
        public MazeSolveReplays replay { get; set; }

        public int Milliseconds { get; set; }
        public int NumberOfMoves { get; set; }
        public int FewestNumberOfMoves { get; set; }
        public int NumberOfActions { get; set; }
        string SystemInfo { get; set; }
    }
}
