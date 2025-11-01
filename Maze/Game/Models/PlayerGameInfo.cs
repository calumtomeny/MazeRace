using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models;
using Maze.Engine.Models;

namespace Maze.Game.Models
{
    public class PlayerGameInfo
    {
        public PlayerGameInfo(Cell startingCell, Cell targetCell, PlayerNumber playerNumber)
        {
            this.CurrentCell = startingCell;
            this.NeedsDrawing = true;
            this.Solved = false;
            this.TargetCell = targetCell;
            this.PlayerNumber = playerNumber;
        }

        public bool hasSolved
        {
            get
            {
                return TargetCell == CurrentCell;
            }
        }

        public TimeSpan GameTimeSpanAtLastMoveInMilliseconds { get; set; }
        public PlayerNumber PlayerNumber { get; private set; }
        public bool NeedsDrawing { get; set; }
        public bool Solved { get; set; }
        public Cell CurrentCell { get; set; }
        public Cell PreviousCell { get; set; }
        public Cell TargetCell { get; set; }
        public int Actions { get; set; }
        public int Moves { get; set; }
        public PlayerGameInfo MovedFromPlayer { get; set; }
        public PlayerType PlayerType { get; set; }

        public int TimeTakenInMilliseconds { get; set; }
    }
}
