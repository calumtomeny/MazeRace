using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using Maze.Game;
using ConsoleUtilities;

namespace Maze
{
    class Program
    {
        static void Main(string[] args)
        {        
            GameHandler gameHandler = new GameHandler();
            gameHandler.DisplaySplashScreen();
            gameHandler.HandleModes();
        }
    }

}

