using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models;

namespace Maze.Game.Models
{
    public class PlayerGameInfo
    {
        public PlayerGameInfo(Cell startingCell)
        {
            currentCell = startingCell;
            needsDrawing = true;
            solved = false;
        }

        public bool needsDrawing { get; set; }
        public bool solved { get; set; }
        public Cell currentCell { get; set; }
        public Cell previousCell { get; set; }
        public int Actions { get; set; }
        public int Moves { get; set; }
        public PlayerGameInfo MovedFromPlayer { get; set; }
    }
}
