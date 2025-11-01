using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Interfaces;
using Maze.Models;
using System.Diagnostics;
using Maze.Game.Models;
using Maze.Engine.Models;
using Maze.Models.Engine;
using Newtonsoft.Json;
using ConsoleUtilities;

namespace Maze.Game.Modes
{
    class TenLevelSpeedRun : IGameMode
    {  
        public TenLevelSpeedRun(Player player)
        {
            CurrentPlayer = player;
            scoreHandler = new ScoreHandler();
            mazeEngine = new MazeGameEngine();
        }

        public Player CurrentPlayer { get; set; }
        private ScoreHandler scoreHandler;

        private MazeGameEngine mazeEngine;

        bool playGame = true;

        public void Start()
        {
            bool playGame = true;

            List<Option> speedRunMultiChoiceOptions = new List<Option>();
            speedRunMultiChoiceOptions.Add(new Option { OptionText = "Start Game", OptionValue = "StartGame" });
            speedRunMultiChoiceOptions.Add(new Option { OptionText = "Top Scores", OptionValue = "TopScores" });
            speedRunMultiChoiceOptions.Add(new Option { OptionText = "Play Replay", OptionValue = "PlayReplay" });
            MultiChoice speedRunMultiChoice = new MultiChoice(speedRunMultiChoiceOptions);

            while (playGame)
            {
                string speedRunOptionResult = speedRunMultiChoice.Show();
                switch (speedRunOptionResult)
                {
                    case "StartGame":
                        {
                            StartGame();
                        }
                        break;
                    case "TopScores":
                        {
                            ShowHighScores();
                        }
                        break;
                    case "PlayReplay":
                        {
                            int replayId;
                            Console.WriteLine("Please enter the replay number: ");
                            while(!int.TryParse(Console.ReadLine(),out replayId))
                            {
                                Console.WriteLine("Please enter a valid number.");
                            }
                            MazeRaceScore score = null;
                            try
                            {
                                score = scoreHandler.GetScoreById(replayId);
                                Console.Clear();
                                PlaySpeedRun(score);
                            }
                            catch
                            {
                                Console.WriteLine(" There was a problem loading the replay.");
                            }
                            Console.WriteLine(" Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;
                    case "ESCAPE":
                        {
                            playGame = false;
                        }
                        break;
                }
            }
        }

        public void StartGame()
        {
            MazeCreationResult mazeGenerationResult;

            MazeSolveReplays gameReplay = new MazeSolveReplays();

            int totalNumberOfMoves = 0;
            int totalNumberOfActions = 0;
            int fewestPossibleMoves = 0;
            long totalTimeInMilliseconds = 0;

            while (playGame)
            {
                int width = 5;
                int height = 5;

                for (int i = 0; i < 10; i++)
                {
                    CompleteSolveInfo solveInfo = null;

                    Console.Clear();
                    Console.WriteLine("Level:" + (i + 1));

                    mazeGenerationResult = mazeEngine.CreateNewMaze(width + i, height + i);

                    fewestPossibleMoves += mazeGenerationResult.solutionPath;

                    solveInfo = mazeEngine.StartSolve(new StartSolveInfo(PlayerType.Human, PlayerType.None, PlayerType.None, PlayerType.None, Difficulty.Medium));

                    if (!solveInfo.EndGame)
                    {
                        HumanSolveResult humanResult = solveInfo.solveResults.First() as HumanSolveResult;

                        totalNumberOfMoves += humanResult.NumberOfMoves;
                        totalNumberOfActions += humanResult.NumberOfActions;
                        totalTimeInMilliseconds += solveInfo.TimeTakenInMilliseconds;

                        gameReplay.replays.Add(solveInfo.replay);
                    }
                    else
                    {
                        fewestPossibleMoves = 0;
                        gameReplay.replays.Clear();
                        totalNumberOfActions = 0;
                        totalNumberOfMoves = 0;
                        totalTimeInMilliseconds = 0;
                        i = -1;
                    }
                }

                MazeSolveReplays replay = gameReplay;

                Console.Clear();
                playGame = false;
                SubmitScore(new GameResult { Replay = Crypto.EncryptStringAES(JsonConvert.SerializeObject(replay), "MazeRace"), TimeTakenInMilliseconds = totalTimeInMilliseconds, FewestPossibleNumberOfMoves = fewestPossibleMoves, TotalNumberOfActions = totalNumberOfActions, TotalNumberOfMoves = totalNumberOfMoves, PlayerName = CurrentPlayer.PlayerName });
                Console.Clear();
            }
            playGame = true;
        }

        private void PlaySpeedRun(MazeRaceScore scoreDetails)
        {
            Console.WriteLine(scoreDetails.PlayerName);
            Console.WriteLine(scoreDetails.Created.ToShortTimeString());
            MazeSolveReplays replay = JsonConvert.DeserializeObject<MazeSolveReplays>(scoreDetails.Replay);
            mazeEngine.PlayReplays(replay, scoreDetails.PlayerName, scoreDetails.Created);
            Console.Clear();
        }

        public void ShowHighScores()
        {
            bool showScores = true;

            while (showScores)
            {
                int topPadding = 1;
                List<MazeRaceScore> topScores = null;
                try
                {
                    Console.WriteLine(" Loading high scores...");
                    ScoreHandler scoreHandler = new ScoreHandler();
                    topScores = scoreHandler.GetTopTenScores();
                    Console.Clear();
                    Console.SetCursorPosition(3, 0 + topPadding);
                    Console.WriteLine(" Top 10 Quickest Times (seconds):");

                    Console.SetCursorPosition(3, 2 + topPadding);
                    Console.Write("");
                    Console.SetCursorPosition(10, 2 + topPadding);
                    Console.Write("Name");
                    Console.SetCursorPosition(22, 2 + topPadding);
                    Console.Write("Time");
                    Console.SetCursorPosition(34, 2 + topPadding);
                    Console.Write("Efficiency");
                    Console.SetCursorPosition(46, 2 + topPadding);
                    Console.Write("Solution Length");

                    for (int i = 0; i < topScores.Count; i++)
                    {
                        Console.SetCursorPosition(3, 4 + i + topPadding);
                        Console.Write(i + 1 + ".");
                        Console.SetCursorPosition(10, 4 + i + topPadding);
                        Console.Write(topScores[i].PlayerName);
                        Console.SetCursorPosition(22, 4 + i + topPadding);
                        Console.Write((double)topScores[i].TimeTakenInMilliseconds / 1000);
                        Console.SetCursorPosition(34, 4 + i + topPadding);
                        Console.Write(String.Format("{0:0.##}", (((float)topScores[i].FewestPossibleNumberOfMoves / (float)topScores[i].TotalNumberOfMoves) * 100)) + "%");
                        Console.SetCursorPosition(46, 4 + i + topPadding);
                        Console.Write(topScores[i].FewestPossibleNumberOfMoves);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine(" Press a number from 0-9 to play a replay or press any other key to continue...");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (char.IsDigit(key.KeyChar))
                    {
                        int index = int.Parse(key.KeyChar.ToString());
                        PlaySpeedRun(topScores[index]);
                    }
                    else
                    {
                        showScores = false;
                    }

                }
                catch (Exception e)
                {
                    Console.Clear();
                    showScores = false;
                    Console.WriteLine(" Could not retrieve top scores at this time.");
                    Console.WriteLine(" Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        public void SubmitScore(GameResult gameResult)
        {
            SubmitScoreResult scoreSubmitResult = null;
            try
            {
                //string scoreSubmitResult = scoreHandler.SubmitScore(CurrentPlayer.PlayerName, Crypto.EncryptStringAES(gameResult.TimeTakenInMilliseconds.ToString(), "Password!"), gameResult.TotalNumberOfMoves, gameResult.FewestPossibleNumberOfMoves, gameResult.TotalNumberOfActions, Crypto.EncryptStringAES(CurrentPlayer.Info, "Password!"));
                scoreSubmitResult = scoreHandler.SubmitGameResult(gameResult);
                if (!scoreSubmitResult.TopTen)
                {
                    Console.WriteLine();
                    Console.WriteLine(" Your score sucked!");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine(" Congratulations, new high score!");
                    Console.WriteLine("");
                }
            }
            catch
            {
                Console.WriteLine("Could not submit score at this time...");
            }
            Console.WriteLine(" Your time: " + String.Format("{0:0.##}", gameResult.TimeTakenInMilliseconds / 1000) + " seconds! With an efficiency of: " + String.Format("{0:0.##}", ((float)gameResult.FewestPossibleNumberOfMoves / (float)gameResult.TotalNumberOfMoves) * 100) + "%");
            Console.WriteLine("");
            Console.WriteLine(" Your replay number is: " + scoreSubmitResult.ScoreId);
            Console.WriteLine("");
            Console.WriteLine("");

            Console.WriteLine(" Press spacebar to continue...");

            while (Console.ReadKey().Key != ConsoleKey.Spacebar) { }
        }
        public void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
