using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Engine.Models
{
    [Serializable]
    public class MazeSolveReplays
    {
        public MazeSolveReplays()
        {
            replays = new List<MazeSolveReplay>();
        }
        public List<MazeSolveReplay> replays { get; set; }
    }
}
