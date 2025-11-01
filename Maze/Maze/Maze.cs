using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Maze.Models;
using Maze.Game.Models;
using System.Threading.Tasks;

namespace Maze
{
    public abstract class MazeBase
    {
        public Cell[][] grid{get; set;}
        public int padding = 4;
        protected MazeBase(int width, int height)
        {
            grid = new Cell[width][];
            for (int i = 0; i < width; i++)
            {
                grid[i] = new Cell[height];
                for (int j = 0; j < height; j++)
                {
                    grid[i][j] = new Cell(i, j);
                }
            }
        }

        public int GetWidth()
        {
            return grid.GetLength(0);
        }

        public int GetHeight()
        {
            return grid[0].GetLength(0);
        }

        public Cell GetBottomLeft()
        {
            return grid[0][GetHeight() - 1];
        }
        public Cell GetTopRight()
        {
            return grid[GetWidth() -1][0];
        }
        public Cell GetTopLeft()
        {
            return grid[0][0];
        }
        public Cell GetBottomRight()
        {
            return grid[GetWidth() - 1][GetHeight() - 1];
        }

        public virtual void DrawRoom(Cell cellToDraw){}

        public virtual void DrawMaze(){}

        public void Generate()
        {
            Random random = new Random();
            Stack<Cell> cellStack = new Stack<Cell>();
            int totalCells = GetWidth() * GetHeight();
            Cell currentCell = grid[random.Next(0, GetWidth())][random.Next(0, GetHeight())];
            Cell previousCell = null;
            int visitedCells = 1;
            DrawMaze();
            while (visitedCells < totalCells)
            {
                //Find all neighbors of CurrentCell with all walls intact
                List<Cell> neighbouringCellsWithAllWalls = new List<Cell>();
                int moo = GetHeight() - 2;
                if (currentCell.Column < GetWidth() - 1 && grid[currentCell.Column + 1][currentCell.Row].HasAllWalls())
                    neighbouringCellsWithAllWalls.Add(grid[currentCell.Column + 1][currentCell.Row]);
                if (currentCell.Column > 0 && grid[currentCell.Column - 1][currentCell.Row].HasAllWalls())
                    neighbouringCellsWithAllWalls.Add(grid[currentCell.Column - 1][currentCell.Row]);
                if (currentCell.Row < GetHeight() - 1 && grid[currentCell.Column][currentCell.Row + 1].HasAllWalls())
                    neighbouringCellsWithAllWalls.Add(grid[currentCell.Column][currentCell.Row + 1]);
                if (currentCell.Row > 0 && grid[currentCell.Column][currentCell.Row - 1].HasAllWalls())
                    neighbouringCellsWithAllWalls.Add(grid[currentCell.Column][currentCell.Row - 1]);

                //if one or more found 
                //choose one at random  
                //knock down the wall between it and CurrentCell  
                //push CurrentCell location on the CellStack  
                if (neighbouringCellsWithAllWalls.Count > 0)
                {
                    Cell randomCell = neighbouringCellsWithAllWalls[random.Next(neighbouringCellsWithAllWalls.Count)];

                    //if random cell is below
                    if (randomCell.Column == currentCell.Column && randomCell.Row == currentCell.Row + 1)
                    {
                        currentCell.KnockDownWall(Wall.Bottom);
                        randomCell.KnockDownWall(Wall.Top);
                    }
                    //if random cell is above
                    if (randomCell.Column == currentCell.Column && randomCell.Row == currentCell.Row - 1)
                    {
                        currentCell.KnockDownWall(Wall.Top);
                        randomCell.KnockDownWall(Wall.Bottom);
                    }
                    //if random cell is to the right
                    if (randomCell.Row == currentCell.Row && randomCell.Column == currentCell.Column + 1)
                    {
                        currentCell.KnockDownWall(Wall.Right);
                        randomCell.KnockDownWall(Wall.Left);
                    }
                    //if random cell is to the left
                    if (randomCell.Row == currentCell.Row && randomCell.Column == currentCell.Column - 1)
                    {
                        currentCell.KnockDownWall(Wall.Left);
                        randomCell.KnockDownWall(Wall.Right);
                    }

                    //make the new cell CurrentCell  
                    previousCell = currentCell;
                    currentCell = randomCell;
                    //add 1 to VisitedCells
                    visitedCells++;
                    cellStack.Push(currentCell);
                }
                else
                {
                    currentCell = cellStack.Pop();
                }
                
                //else 
                //pop the most recent cell entry off the CellStack  
                //make it CurrentCell

                DrawRoom(currentCell);
                DrawRoom(previousCell);
                Console.SetCursorPosition(currentCell.Column * 2 + padding + 1, currentCell.Row + padding + 1);
            }
        }
      
        public Cell MoveCell(Direction directionToMove, PlayerGameInfo playerInfo)
        {
            playerInfo.Moves++;
            switch(directionToMove)
            {
                case Direction.Up:
                    return grid[playerInfo.CurrentCell.Column][playerInfo.CurrentCell.Row - 1];
                case Direction.Right:
                    return grid[playerInfo.CurrentCell.Column + 1][playerInfo.CurrentCell.Row];
                case Direction.Down:
                    return grid[playerInfo.CurrentCell.Column][playerInfo.CurrentCell.Row + 1];
                default:
                    return grid[playerInfo.CurrentCell.Column - 1][playerInfo.CurrentCell.Row];
            }                
        }

        public string SerializeMaze()
        {
            return grid.ToJSONString();
        }
    }
}
