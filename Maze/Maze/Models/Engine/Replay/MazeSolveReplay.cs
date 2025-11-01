using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models;

namespace Maze.Engine.Models
{
    public class MazeSolveReplay
    {
        public MazeSolveReplay()
        {
            ReplayMoves = new List<ReplayMove>();
        }

        public Cell[][] Grid { get; set; }
        public List<ReplayMove> ReplayMoves { get; set; }
        public int NumberOfPlayers { get; set; }
    }
}
