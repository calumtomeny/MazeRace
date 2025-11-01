using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Game.Models
{
    public class MazeCreationResult
    {
        public int solutionPath { get; private set; }

        public MazeCreationResult(int solutionPath)
        {
            this.solutionPath = solutionPath; 
        }
    }
}
