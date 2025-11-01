using System;
using System.Collections.Generic;
using System.Diagnostics;
using Maze.Models;
using Maze.Game.Interfaces;
using Maze.Game.Modes;
using ConsoleUtilities;
using Maze.Models.Engine;
using Maze.Engine.Models;

namespace Maze.Game
{
    public class GameHandler
    {
        public Player CurrentPlayer { get; set; }
        private ScoreHandler scoreHandler;

        public GameHandler()
        {
            CurrentPlayer = new Player();
            scoreHandler = new ScoreHandler();
        }

        public void DisplaySplashScreen()
        {
            Console.WriteLine(@" ___  ___  ___  ____  ____    ____   ___    ___  ____");
            Console.WriteLine(@" ||\\//|| // \\   // ||       || \\ // \\  //   || ");
            Console.WriteLine(@" || \/ || ||=||  //  ||==     ||_// ||=|| ((    ||== ");
            Console.WriteLine(@" ||    || || || //__ ||___    || \\ || ||  \\__ ||___");
            Console.WriteLine("");
            Console.WriteLine("");
        }

        public void DisplayInstructions()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Maze Race!");
            Console.WriteLine();
            Console.WriteLine("The objective of this game is to traverse each maze from the bottom left");
            Console.WriteLine("corner to the upper right corner as fast as possible. There are");
            Console.WriteLine("ten levels to complete.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Controls:");
            Console.WriteLine("-Arrow Keys to move");
            Console.WriteLine("-Hold 'CTRL' and an arrow key to move as far as possible in that direction");
            Console.WriteLine("-Hold 'Shift' and a direction to move to the next choice point");
            Console.WriteLine();
            Console.WriteLine();
            Pause();
            Console.Clear();
        }

        public void GetPlayerName()
        {
            Console.Write(" Please enter your name (six characters max): ");
            CurrentPlayer.PlayerName = Console.ReadLine();

            while (CurrentPlayer.PlayerName.Length > 6 && CurrentPlayer.PlayerName.Length <3)
            {
                Console.WriteLine(" Please enter your name (six characters max, three character min):");
                CurrentPlayer.PlayerName = Console.ReadLine();
            }
        }
        public void HandleModes()
        {
            bool playGame = true;

            GetPlayerName();

            List<Option> mainMultiChoiceOptions = new List<Option>();
            mainMultiChoiceOptions.Add(new Option { OptionText = "Ten Level Speed Run", OptionValue = "TenLevelSpeedRun" });
            mainMultiChoiceOptions.Add(new Option { OptionText = "Race Computer", OptionValue = "RaceComputer" });
            mainMultiChoiceOptions.Add(new Option { OptionText = "Two Player Game", OptionValue = "TwoPlayerGame" });
            mainMultiChoiceOptions.Add(new Option { OptionText = "Help", OptionValue = "Help" });
            mainMultiChoiceOptions.Add(new Option { OptionText = "Quit", OptionValue = "Quit" });
            MultiChoice mainMultiChoice = new MultiChoice(mainMultiChoiceOptions);

            //IGameMode TenLevelSpeed = new TenLevelSpeedRun(CurrentPlayer);
            //IGameMode TwoPlayerGame = new TwoPlayerSpeedRun();

            while (playGame)
            {
                string result = mainMultiChoice.Show();
                switch (result)
                {
                    case "TenLevelSpeedRun":
                        {
                            IGameMode TenLevelSpeedRun = new TenLevelSpeedRun(new Player { PlayerName = CurrentPlayer.PlayerName });
                            TenLevelSpeedRun.Start();
                        }
                        break;
                    case "TwoPlayerGame":
                        {
                            IGameMode TwoPlayerGame = new TwoPlayerSpeedRun();
                            TwoPlayerGame.Start();
                        }
                        break;
                    case "RaceComputer":
                        {
                            RenderCPUMultiChoiceOptions();
                        }
                        break;
                    case "Help":
                        DisplayInstructions();
                        break;
                    case "Quit":
                        playGame = false;
                        break;
                }
            }
        }

        public void RenderCPUMultiChoiceOptions()
        {
            IGameMode RaceComputer = null;

            List<Option> cpuRaceOptions = new List<Option>();
            cpuRaceOptions.Add(new Option { OptionText = "Easy", OptionValue = "Easy" });
            cpuRaceOptions.Add(new Option { OptionText = "Medium", OptionValue = "Medium" });
            cpuRaceOptions.Add(new Option { OptionText = "Hard", OptionValue = "Hard" });
            cpuRaceOptions.Add(new Option { OptionText = "Legendary", OptionValue = "Legendary" });
            cpuRaceOptions.Add(new Option { OptionText = "Godlike", OptionValue = "Godlike" });
            MultiChoice cpuRaceMultiChoice = new MultiChoice(cpuRaceOptions);

            string result = cpuRaceMultiChoice.Show();
            switch (result)
            {
                case "Easy":
                    {
                        RaceComputer = new RaceComputer(Difficulty.Easy);
                    }
                    break;
                case "Medium":
                    {
                        RaceComputer = new RaceComputer(Difficulty.Medium);
                    }
                    break;
                case "Hard":
                    {
                        RaceComputer = new RaceComputer(Difficulty.Hard);
                    }
                    break;
                case "Legendary":
                    {
                        RaceComputer = new RaceComputer(Difficulty.Legendary);
                    }
                    break;
                case "Godlike":
                    {
                        RaceComputer = new RaceComputer(Difficulty.Godlike);
                    }
                    break;
            }
            RaceComputer.Start();
        }

        public void Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
