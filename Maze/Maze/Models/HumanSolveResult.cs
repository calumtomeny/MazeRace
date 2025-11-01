using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Models
{
    public class HumanSolveResult
    {
        public HumanSolveResult()
        {
            solved = false;
        }

        public int NumberOfMoves { get; set; }
        public int NumberOfActions { get; set; }
        public bool solved { get; set; }
    }
}
