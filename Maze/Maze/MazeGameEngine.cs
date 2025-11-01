using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Game.Models;
using Maze.Models;
using System.Threading;
using System.Threading.Tasks;
using Maze.Engine.Models;
using Maze.Models.Engine;
using System.Diagnostics;

namespace Maze
{
    public class MazeGameEngine
    {
        ConsoleMaze maze;
        List<PlayerGameInfo> players;
        Stopwatch gameTimer;
        MazeSolveReplay replay;

        bool replayMode;
        
        public MazeCreationResult CreateNewMaze(int width, int height)
        {
            int solutionLength;
            maze = new ConsoleMaze(width, height);
            maze.Generate();
            solutionLength = GetSolutionLenth();
            return new MazeCreationResult(solutionLength);
        }

        public string GetSerializedMaze()
        {
            return maze.SerializeMaze();
        }

        public int GetSolutionLenth()
        {
            bool solved = false;
            Random random = new Random();
            Stack<Cell> cellStack = new Stack<Cell>();
            List<Cell> visitedCells = new List<Cell>();
            int totalCells = maze.GetWidth() * maze.GetHeight();
            Cell currentCell = maze.grid[0][maze.GetHeight() - 1];
            Cell previousCell = null;
            int numberofCellsVisitedCells = 1;

            bool firstPop = true;

            while (!solved)
            {
                if (numberofCellsVisitedCells == 1)
                    cellStack.Push(currentCell);

                //Find all accesssable neighbors of CurrentCell that are unvisited
                List<Cell> neighbouringCellsWithAllWalls = new List<Cell>();
                if (currentCell.Column < maze.GetWidth() - 1 && !currentCell.HasRightWall())
                {
                    if (!visitedCells.Contains(maze.grid[currentCell.Column + 1][currentCell.Row]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[currentCell.Column + 1][currentCell.Row]);
                }
                if (currentCell.Column > 0 && !currentCell.HasLeftWall())
                {
                    if (!visitedCells.Contains(maze.grid[currentCell.Column - 1][currentCell.Row]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[currentCell.Column - 1][currentCell.Row]);
                }
                if (currentCell.Row < maze.GetHeight() - 1 && !currentCell.HasBottomWall())
                {
                    if (!visitedCells.Contains(maze.grid[currentCell.Column][currentCell.Row + 1]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[currentCell.Column][currentCell.Row + 1]);
                }
                if (currentCell.Row > 0 && !currentCell.HasTopWall())
                {
                    if (!visitedCells.Contains(maze.grid[currentCell.Column][currentCell.Row - 1]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[currentCell.Column][currentCell.Row - 1]);
                }

                //if one or more found 
                //choose one at random  
                //push CurrentCell location on the CellStack 

                if (neighbouringCellsWithAllWalls.Count > 0)
                {
                    firstPop = true;
                    previousCell = currentCell;
                    //Push the current cell onto the stack if it isn't the top cell so that the count of the stack reflects the solution length
                    if (cellStack.Count == 0 || cellStack.Peek() != currentCell)
                        cellStack.Push(currentCell);

                    Cell randomCell = neighbouringCellsWithAllWalls[random.Next(neighbouringCellsWithAllWalls.Count)];

                    visitedCells.Add(currentCell);

                    //make the new cell CurrentCell  
                    currentCell = randomCell;
                    cellStack.Push(currentCell);

                    if (currentCell == maze.grid[maze.GetWidth() - 1][0])
                        solved = true;
                    //add 1 to VisitedCells
                    numberofCellsVisitedCells++;
                }
                else
                {
                    //pop the most recent cell entry off the CellStack  
                    //make it CurrentCell
                    if (firstPop)
                    {
                        previousCell = cellStack.Peek();
                        visitedCells.Add(cellStack.Pop());
                        currentCell = cellStack.Pop();
                        visitedCells.Add(currentCell);
                    }
                    else
                    {
                        previousCell = currentCell;
                        currentCell = cellStack.Pop();
                        visitedCells.Add(currentCell);
                    }
                    firstPop = false;
                }
            }
            return cellStack.Count - 1;
        }

        public CompleteSolveInfo StartSolve(StartSolveInfo startSolveInfo)
        {
            replayMode = false;
            replay = new MazeSolveReplay();
            gameTimer = new Stopwatch();
            gameTimer.Start();
            InitializePlayers(startSolveInfo, out players);
            replay.NumberOfPlayers = players.Count;
            replay.Grid = maze.grid;

            bool endGame = false;

            //Check if a player has solved the maze
            List<SolveResult> solveResults = new List<SolveResult>();

            while (!players.Where(x => x.Solved).Any() && !endGame)
            {               
                foreach (PlayerGameInfo p in players)
                    if (p.CurrentCell == p.TargetCell)
                        p.Solved = true;

                for (int i = 0; i < players.Count; i++)
                {
                    ConsoleColor playerColour = ConsoleColor.Red;
                    if (i == 1)
                        playerColour = ConsoleColor.Blue;
                    RenderPlayerMovement(players, i, playerColour);
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    bool shiftPressed = ((key.Modifiers & ConsoleModifiers.Alt) != 0);
                    bool ctrlPressed = ((key.Modifiers & ConsoleModifiers.Control) != 0);

                    MovementType movementType = MovementType.SingleCell;

                    if (players.Count > 1)
                        movementType = MovementType.NextOption;

                    if (shiftPressed)
                        movementType = MovementType.NextOption;
                    if (ctrlPressed)
                        movementType = MovementType.NextBlock;

                    switch (key.Key)
                    {
                        case ConsoleKey.UpArrow:
                            {
                                HandlePlayerMovement(players[0], new Movement { Type = movementType, Direction = MovementDirection.Up });
                                players[0].Actions++;
                            }
                            break;
                        case ConsoleKey.DownArrow:
                            {
                                HandlePlayerMovement(players[0], new Movement { Type = movementType, Direction = MovementDirection.Down });
                                players[0].Actions++;
                            }
                            break;
                        case ConsoleKey.LeftArrow:
                            {
                                players[0].Actions++;
                                HandlePlayerMovement(players[0], new Movement { Type = movementType, Direction = MovementDirection.Left });
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            {
                                players[0].Actions++;
                                HandlePlayerMovement(players[0], new Movement { Type = movementType, Direction = MovementDirection.Right });
                            }
                            break;
                        case ConsoleKey.W:
                            {
                                if (players.Count > 1)
                                {
                                    players[1].Actions++;
                                    HandlePlayerMovement(players[1], new Movement { Type = movementType, Direction = MovementDirection.Up });
                                }
                            }
                            break;
                        case ConsoleKey.S:
                            if (players.Count > 1)
                            {
                                players[1].Actions++;
                                HandlePlayerMovement(players[1], new Movement { Type = movementType, Direction = MovementDirection.Down });
                            }
                            break;
                        case ConsoleKey.A:
                            if (players.Count > 1)
                            {
                                players[1].Actions++;
                                HandlePlayerMovement(players[1], new Movement { Type = movementType, Direction = MovementDirection.Left });
                            }
                            break;
                        case ConsoleKey.D:
                            if (players.Count > 1)
                            {
                                players[1].Actions++;
                                HandlePlayerMovement(players[1], new Movement { Type = movementType, Direction = MovementDirection.Right });
                            }
                            break;
                        case ConsoleKey.R:
                            endGame = true;
                            break;
                        default:
                            break;
                    }                   
                }
            }
            gameTimer.Stop();
            foreach (PlayerGameInfo p in players)
            {
                PlayerType playerType = p.PlayerType;
                if (playerType == PlayerType.Human)
                    solveResults.Add(new HumanSolveResult(p.Moves, p.TimeTakenInMilliseconds, p.Solved, p.Actions, p.PlayerNumber));
                else
                    solveResults.Add(new ComputerSolveResult(p.Moves, p.TimeTakenInMilliseconds, p.Solved, p.PlayerNumber));
            }
            return new CompleteSolveInfo(solveResults, gameTimer.ElapsedMilliseconds, replay, endGame);
        }

        public void InitializePlayers(StartSolveInfo solveInfo, out List<PlayerGameInfo> players)
        {
            players = new List<PlayerGameInfo>();
            if (solveInfo.player1 != PlayerType.None)
            {
                PlayerGameInfo playerToAdd = new PlayerGameInfo(maze.GetBottomLeft(), maze.GetTopRight(), PlayerNumber.Player1);
                players.Add(playerToAdd);
                if (solveInfo.player1 == PlayerType.CPU)
                {
                    EnablePlayerCpu(playerToAdd, solveInfo.CPUdifficulty);
                    playerToAdd.PlayerType = PlayerType.CPU;
                }
                else
                {
                    playerToAdd.PlayerType = PlayerType.Human;
                }
            }

            if (solveInfo.player2 != PlayerType.None)
            {
                PlayerGameInfo playerToAdd = new PlayerGameInfo(maze.GetTopRight(), maze.GetBottomLeft(), PlayerNumber.Player2);
                players.Add(playerToAdd);
                if (solveInfo.player2 == PlayerType.CPU)
                {
                    EnablePlayerCpu(playerToAdd, solveInfo.CPUdifficulty);
                    playerToAdd.PlayerType = PlayerType.CPU;
                }
                else
                {
                    playerToAdd.PlayerType = PlayerType.Human;
                }
            }
        }

        public void EnablePlayerCpu(PlayerGameInfo player, Difficulty CPUDifficulty)
        {
            Task.Factory.StartNew(() =>
            {
                //If the thread tries to access the maze when it's already been solved then it throws an error, try catch hides it. Not too great.
                try
                {
                    CPUPlayer(player, player.TargetCell, GetCPUMovementIntervalnMilliseconds(CPUDifficulty));
                }
                catch { }
            }).ContinueWith((x) =>
            {
                player.Solved = true;
            });
        }

        private int GetCPUMovementIntervalnMilliseconds(Difficulty solveInfo)
        {
            switch (solveInfo)
            {
                case Difficulty.Easy:
                    return 500;
                case Difficulty.Medium:
                    return 200;
                case Difficulty.Hard:
                    return 130;
                case Difficulty.Legendary:
                    return 90;
                default:
                    return 50;
            }
        }

        public void CPUPlayer(PlayerGameInfo player, Cell goal, int moveIntervalInMilliseconds = 200)
        {
            player.Solved = false;
            Random random = new Random();
            Stack<Cell> cellStack = new Stack<Cell>();
            List<Cell> visitedCells = new List<Cell>();

            int totalCells = maze.GetWidth() * maze.GetHeight();

            player.PreviousCell = player.CurrentCell;
            int numberofCellsVisited = 1;

            bool firstPop = true;

            while (!player.Solved)
            {
                if (numberofCellsVisited == 1)
                    cellStack.Push(player.CurrentCell);

                //Find all accesssable neighbors of CurrentCell that are unvisited
                List<Cell> neighbouringCellsWithAllWalls = new List<Cell>();
                if (player.CurrentCell.Column < maze.GetWidth() - 1 && !player.CurrentCell.HasRightWall())
                {
                    if (!visitedCells.Contains(maze.grid[player.CurrentCell.Column + 1][player.CurrentCell.Row]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[player.CurrentCell.Column + 1][player.CurrentCell.Row]);
                }
                if (player.CurrentCell.Column > 0 && !player.CurrentCell.HasLeftWall())
                {
                    if (!visitedCells.Contains(maze.grid[player.CurrentCell.Column - 1][player.CurrentCell.Row]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[player.CurrentCell.Column - 1][player.CurrentCell.Row]);
                }
                if (player.CurrentCell.Row < maze.GetHeight() - 1 && !player.CurrentCell.HasBottomWall())
                {
                    if (!visitedCells.Contains(maze.grid[player.CurrentCell.Column][player.CurrentCell.Row + 1]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[player.CurrentCell.Column][player.CurrentCell.Row + 1]);
                }
                if (player.CurrentCell.Row > 0 && !player.CurrentCell.HasTopWall())
                {
                    if (!visitedCells.Contains(maze.grid[player.CurrentCell.Column][player.CurrentCell.Row - 1]))
                        neighbouringCellsWithAllWalls.Add(maze.grid[player.CurrentCell.Column][player.CurrentCell.Row - 1]);
                }

                //if one or more found 
                //choose one at random  
                //push CurrentCell location on the CellStack 
                // player.previousCell = player.currentCell;
                if (neighbouringCellsWithAllWalls.Count > 0)
                {
                    firstPop = true;

                    //Push the current cell onto the stack if it isn't the top cell so that the count of the stack reflects the solution length
                    if (cellStack.Count == 0 || cellStack.Peek() != player.CurrentCell)
                        cellStack.Push(player.CurrentCell);

                    Cell randomCell = neighbouringCellsWithAllWalls[random.Next(neighbouringCellsWithAllWalls.Count)];

                    visitedCells.Add(player.CurrentCell);

                    //Move player to random cell

                    if (player.CurrentCell.Column == randomCell.Column && player.CurrentCell.Row > randomCell.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Up, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column < randomCell.Column && player.CurrentCell.Row == randomCell.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Right, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column == randomCell.Column && player.CurrentCell.Row < randomCell.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Down, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column > randomCell.Column && player.CurrentCell.Row == randomCell.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Left, Type = MovementType.SingleCell });

                    cellStack.Push(player.CurrentCell);

                    if (player.CurrentCell == goal)
                        player.Solved = true;
                    //add 1 to VisitedCells
                    numberofCellsVisited++;
                }
                else
                {
                    //pop the most recent cell entry off the CellStack  
                    //make it CurrentCell

                    Cell cellFromStack = null;

                    visitedCells.Add(player.CurrentCell);

                    if (firstPop)
                    {
                        //player.previousCell = cellStack.Peek();
                        visitedCells.Add(cellStack.Pop());
                        cellFromStack = cellStack.Pop();
                        visitedCells.Add(player.CurrentCell);
                    }
                    else
                    {
                        cellFromStack = cellStack.Pop();
                        visitedCells.Add(player.CurrentCell);
                    }

                    if (player.CurrentCell.Column == cellFromStack.Column && player.CurrentCell.Row > cellFromStack.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Up, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column < cellFromStack.Column && player.CurrentCell.Row == cellFromStack.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Right, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column == cellFromStack.Column && player.CurrentCell.Row < cellFromStack.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Down, Type = MovementType.SingleCell });

                    if (player.CurrentCell.Column > cellFromStack.Column && player.CurrentCell.Row == cellFromStack.Row)
                        HandlePlayerMovement(player, new Movement() { Direction = MovementDirection.Left, Type = MovementType.SingleCell });

                    firstPop = false;
                }
                Thread.Sleep(moveIntervalInMilliseconds);
            }
        }

        private void RenderPlayerMovement(List<PlayerGameInfo> players, int playerToDraw, ConsoleColor playerColour)
        {
            bool playerInPreviousCell = false;
            if (players[playerToDraw].NeedsDrawing)
            {
                Console.ForegroundColor = playerColour;

                int columnToDrawPos = players[playerToDraw].CurrentCell.Column * 2 + maze.padding + 1;
                int rowToDrawPos = players[playerToDraw].CurrentCell.Row + maze.padding + 1;

                Console.SetCursorPosition(columnToDrawPos, rowToDrawPos);
                Console.Write('\u2588');
                Console.ForegroundColor = ConsoleColor.Gray;

                //Check if there is a player in the previous cell of the current player
                foreach (PlayerGameInfo player in players)
                {
                    if (player.CurrentCell == players[playerToDraw].PreviousCell)
                        playerInPreviousCell = true;
                }

                //Draw player if a previous cell exists and there isn't a player in the previous cell
                if (players[playerToDraw].PreviousCell != null && !playerInPreviousCell)
                    maze.DrawRoom(players[playerToDraw].PreviousCell);

                players[playerToDraw].NeedsDrawing = false;
            }
        }

        private void HandlePlayerMovement(PlayerGameInfo playerInfo, Movement movement)
        {
            if (!replayMode)
            {
                replay.ReplayMoves.Add(new ReplayMove { PlayerMovement = movement, PlayerNumber = playerInfo.PlayerNumber, TimeSpanBeforeMovement = gameTimer.Elapsed.Subtract(playerInfo.GameTimeSpanAtLastMoveInMilliseconds) });
                playerInfo.GameTimeSpanAtLastMoveInMilliseconds = gameTimer.Elapsed;
            }
            if (movement.Direction == MovementDirection.Up)
            {
                if (playerInfo.CurrentCell.Row > 0 && !playerInfo.CurrentCell.HasTopWall())
                {
                    playerInfo.NeedsDrawing = true;
                    playerInfo.PreviousCell = playerInfo.CurrentCell;
                    if (movement.Type != MovementType.SingleCell)
                    {
                        bool canKeepGoingUp = true;
                        while (canKeepGoingUp)
                        {
                            if (playerInfo.CurrentCell.Row > 0 && !playerInfo.CurrentCell.HasTopWall())
                            {
                                playerInfo.CurrentCell = maze.MoveCell(Direction.Up, playerInfo);
                                if (movement.Type == MovementType.NextOption)
                                {
                                    if (!playerInfo.CurrentCell.HasLeftWall() || !playerInfo.CurrentCell.HasRightWall())
                                        canKeepGoingUp = false;
                                }
                            }
                            else
                                canKeepGoingUp = false;
                        }
                    }
                    else
                    {
                        playerInfo.CurrentCell = maze.MoveCell(Direction.Up, playerInfo);
                    }
                }
            }
            if (movement.Direction == MovementDirection.Down)
            {
                if (playerInfo.CurrentCell.Row < maze.GetHeight() - 1 && !playerInfo.CurrentCell.HasBottomWall())
                {
                    playerInfo.NeedsDrawing = true;
                    playerInfo.PreviousCell = playerInfo.CurrentCell;
                    if (movement.Type != MovementType.SingleCell)
                    {
                        bool canKeepGoingDown = true;
                        while (canKeepGoingDown)
                        {
                            if (playerInfo.CurrentCell.Row < maze.GetHeight() - 1 && !playerInfo.CurrentCell.HasBottomWall())
                            {
                                playerInfo.CurrentCell = maze.MoveCell(Direction.Down, playerInfo);
                                if (movement.Type == MovementType.NextOption)
                                {
                                    if (!playerInfo.CurrentCell.HasLeftWall() || !playerInfo.CurrentCell.HasRightWall())
                                        canKeepGoingDown = false;
                                }
                            }
                            else
                                canKeepGoingDown = false;
                        }
                    }
                    else
                    {
                        playerInfo.CurrentCell = maze.MoveCell(Direction.Down, playerInfo);
                    }
                }
            }
            if (movement.Direction == MovementDirection.Left)
            {
                if (playerInfo.CurrentCell.Column > 0 && !playerInfo.CurrentCell.HasLeftWall())
                {
                    playerInfo.NeedsDrawing = true;
                    playerInfo.PreviousCell = playerInfo.CurrentCell;
                    if (movement.Type != MovementType.SingleCell)
                    {
                        bool canKeepGoingLeft = true;
                        while (canKeepGoingLeft)
                        {
                            if (playerInfo.CurrentCell.Column > 0 && !playerInfo.CurrentCell.HasLeftWall())
                            {
                                playerInfo.CurrentCell = maze.MoveCell(Direction.Left, playerInfo);
                                if (movement.Type == MovementType.NextOption)
                                {
                                    if (!playerInfo.CurrentCell.HasTopWall() || !playerInfo.CurrentCell.HasBottomWall())
                                        canKeepGoingLeft = false;
                                }
                            }
                            else
                                canKeepGoingLeft = false;
                        }
                    }
                    else
                    {
                        playerInfo.CurrentCell = maze.MoveCell(Direction.Left, playerInfo);
                    }
                }
            }
            if (movement.Direction == MovementDirection.Right)
            {
                if (playerInfo.CurrentCell.Column < maze.GetWidth() - 1 && !playerInfo.CurrentCell.HasRightWall())
                {
                    playerInfo.NeedsDrawing = true;
                    playerInfo.PreviousCell = playerInfo.CurrentCell;
                    if (movement.Type != MovementType.SingleCell)
                    {
                        bool canKeepGoingRight = true;
                        while (canKeepGoingRight)
                        {
                            if (playerInfo.CurrentCell.Column < maze.GetWidth() - 1 && !playerInfo.CurrentCell.HasRightWall())
                            {
                                playerInfo.CurrentCell = maze.MoveCell(Direction.Right, playerInfo);
                                if (movement.Type == MovementType.NextOption)
                                {
                                    if (!playerInfo.CurrentCell.HasTopWall() || !playerInfo.CurrentCell.HasBottomWall())
                                        canKeepGoingRight = false;
                                }
                            }
                            else
                                canKeepGoingRight = false;
                        }
                    }
                    else
                    {
                        playerInfo.CurrentCell = maze.MoveCell(Direction.Right, playerInfo);
                    }
                }
            }
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < players.Count; j++)
                {
                    if (i != j)
                    {
                        if (players[i].PreviousCell == players[j].CurrentCell)
                            players[j].NeedsDrawing = true;
                    }
                }
            }
        }

        public void PlayReplays(MazeSolveReplays gameReplay, string playerName, DateTime gameDate)
        {
            replayMode = true;

            maze = new ConsoleMaze(0, 0);
            players = new List<PlayerGameInfo>();

            //Play every replay in collection
            foreach (MazeSolveReplay replay in gameReplay.replays.ToList())
            {
                bool watchReplay = true;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                Console.WriteLine(" Replay!");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine(" Name: " + playerName);
                Console.WriteLine(" Date: " + gameDate);
                maze.grid = replay.Grid;

                if (replay.NumberOfPlayers == 1)
                {
                    players.Clear();  
                    PlayerGameInfo playerToAdd = new PlayerGameInfo(maze.GetBottomLeft(), maze.GetTopRight(), PlayerNumber.Player1);
                    players.Add(playerToAdd);

                    maze.DrawMaze();
                 
                    foreach(ReplayMove r in replay.ReplayMoves.ToList())
                    {
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.Escape)
                            {
                                watchReplay = false;
                                break;
                            }
                        }

                        RenderPlayerMovement(players, 0, ConsoleColor.Red);
                        Thread.Sleep(r.TimeSpanBeforeMovement);
                        HandlePlayerMovement(players[0], r.PlayerMovement);
                    }
                    if (!watchReplay)
                        break;
                }
            }
        }
    }
}
