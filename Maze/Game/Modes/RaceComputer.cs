using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Interfaces;
using Maze.Models;
using System.Diagnostics;
using Maze.Game.Models;
using Maze.Models.Engine;
using Maze.Engine.Models;

namespace Maze.Game.Modes
{
    class RaceComputer : IGameMode
    {
        public RaceComputer(Difficulty CPUdifficulty)
        {
            scoreHandler = new ScoreHandler();
            mazeEngine = new MazeGameEngine();
            this.CPUdifficulty = CPUdifficulty;
        }

        public Difficulty CPUdifficulty { get; private set; }
        public Player CurrentPlayer { get; set; }
        private ScoreHandler scoreHandler;

        private MazeGameEngine mazeEngine;

        bool playGame = true;

        public void Start()
        {
            MazeCreationResult mazeGenerationResult;
            Stopwatch stopWatch = new Stopwatch();
            int playerOneScore = 0;
            int playerTwoScore = 0;
            while (playGame)
            {
                int width = 5;
                int height = 5;

                    playerOneScore = 0;
                    playerTwoScore = 0;
                    stopWatch.Restart();
                    for (int i = 0; i < 5; i++)
                    {
                        Console.Clear();
                        Console.WriteLine("Level:" + (i + 1));
                        Console.WriteLine("Player One Score: " + playerOneScore);
                        Console.WriteLine("Player Two SCore: " + playerTwoScore);
                        stopWatch.Stop();
                        mazeGenerationResult = mazeEngine.CreateNewMaze(width + i, height + i);
                       
                        stopWatch.Start();

                        CompleteSolveInfo solveInfo = mazeEngine.StartSolve(new StartSolveInfo(PlayerType.Human, PlayerType.CPU, PlayerType.None, PlayerType.None, CPUdifficulty));

                        if (solveInfo.solveResults.Where(x => x.PlayerNumber == PlayerNumber.Player1).Single().Solved)
                            playerOneScore++;
                        else
                            playerTwoScore++;
                    }
                stopWatch.Stop();
                TimeSpan totalTime = stopWatch.Elapsed;
                Console.Clear();
                playGame = false;
            }
        }
    }
}
