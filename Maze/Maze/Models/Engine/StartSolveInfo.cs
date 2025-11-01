using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Engine.Models
{
    public class StartSolveInfo
    {
        public StartSolveInfo(PlayerType player1, PlayerType player2, PlayerType player3, PlayerType player4, Difficulty CPUdifficulty)
        {
            this.player1 = player1;
            this.player2 = player2;
            this.player3 = player3;
            this.player4 = player4;
            this.CPUdifficulty = CPUdifficulty;
        }

        public PlayerType player1 { get; private set; }
        public PlayerType player2 { get; private set; }
        public PlayerType player3 { get; private set; }
        public PlayerType player4 { get; private set; }
        public Difficulty CPUdifficulty { get; private set; }
    }
}
