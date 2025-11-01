using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models;
using Maze.Game.Models;

namespace Maze.Engine.Models
{
    public class ReplayMove
    {
        public TimeSpan TimeSpanBeforeMovement { get; set; }
        public Movement PlayerMovement { get; set; }
        public PlayerNumber PlayerNumber { get; set; }
    }
}
