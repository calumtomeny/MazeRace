using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Models
{
    public class Player
    {
        public Player()
        {
            Info =  System.Environment.MachineName + " " + System.Environment.UserName + " " + ((System.Environment.TickCount / 1000) / 60) / 60 + " " + System.Environment.OSVersion;
        }
        public string PlayerName { get; set; }
        public string Info { get; private set; }
    }
}
