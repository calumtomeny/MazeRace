using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Models
{
    public class ScoreModel
    {
        public string Name { get; set; }
        public int TimeTaken { get; set; }
        public int NumberOfMoves { get; set; }
        public int FewestNumberOfMoves { get; set; }
        public int NumberOfActions { get; set; }
        public string SystemInfo { get; set; }
    }
}
