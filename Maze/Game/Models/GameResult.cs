using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Engine.Models;

namespace Maze
{
    public class GameResult
    {
        public string PlayerName { get; set; }
        public int TotalNumberOfMoves { get; set; }
        public int TotalNumberOfActions { get; set; }
        public int FewestPossibleNumberOfMoves { get; set; }
        public long TimeTakenInMilliseconds { get; set; }
        public string Replay { get; set; }
    }
}
