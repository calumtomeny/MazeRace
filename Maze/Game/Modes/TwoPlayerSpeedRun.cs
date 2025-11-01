using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Interfaces;
using Maze.Models;
using System.Diagnostics;
using Maze.Engine.Models;
using Maze.Models.Engine;

namespace Maze.Game.Modes
{
    class TwoPlayerSpeedRun : IGameMode
    {
        public TwoPlayerSpeedRun()
        {
            scoreHandler = new ScoreHandler();
            mazeEngine = new MazeGameEngine();
        }

        public Player CurrentPlayer { get; set; }
        private ScoreHandler scoreHandler;

        private MazeGameEngine mazeEngine;

        bool playGame = true;

        public void Start()
        {
            Stopwatch stopWatch = new Stopwatch();
            int playerOneScore = 0;
            int playerTwoScore = 0;
            while (playGame)
            {
                bool restart = false;
                int width = 5;
                int height = 5;
                do
                {
                    playerOneScore = 0;
                    playerTwoScore = 0;
                    restart = false;
                    stopWatch.Restart();
                    for (int i = 0; i < 20; i++)
                    {
                        Console.Clear();
                        Console.WriteLine("Level:" + (i + 1));
                        Console.WriteLine("Player One Score: " + playerOneScore);
                        Console.WriteLine("Player Two SCore: " + playerTwoScore);
                        stopWatch.Stop();
                        mazeEngine.CreateNewMaze(width + i, height + i);
                        stopWatch.Start();

                         CompleteSolveInfo solveInfo = mazeEngine.StartSolve(new StartSolveInfo(PlayerType.Human, PlayerType.Human, PlayerType.None, PlayerType.None, Difficulty.Medium));

                        if (solveInfo.solveResults.ToList()[0].Solved)
                            playerOneScore++;
                        else
                            playerTwoScore++;
                    }
                }
                while (restart);
                stopWatch.Stop();
                TimeSpan totalTime = stopWatch.Elapsed;
                Console.Clear();
            }
        }
    }
}
