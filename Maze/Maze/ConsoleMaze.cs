using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Models;

namespace Maze
{
    public class ConsoleMaze : MazeBase
    {
        public ConsoleMaze(int width, int height)
            : base(width, height)
        {
        }
        public override void DrawRoom(Cell cellToDraw)
        {
            //int k = i == 0 ? i : i / 2;
            //current = grid[k, j];
            if (cellToDraw.HasRightWall())
            {
                Console.SetCursorPosition(cellToDraw.Column * 2 + 2 + padding, cellToDraw.Row + padding + 1);
                Console.Write("|");
            }
            else
            {
                if (cellToDraw.Row == GetHeight() - 1)
                {
                    Console.SetCursorPosition(cellToDraw.Column * 2 + 2 + padding, cellToDraw.Row + padding + 1);
                    Console.Write("_");
                }
                else if (grid[cellToDraw.Column][cellToDraw.Row + 1].HasTopWall())
                {
                    Console.SetCursorPosition(cellToDraw.Column * 2 + 2 + padding, cellToDraw.Row + padding + 1);
                    Console.Write("_");
                }
                else
                {
                    Console.SetCursorPosition(cellToDraw.Column * 2 + 2 + padding, cellToDraw.Row + padding + 1);
                    Console.Write(" ");
                }
            }
            //Thread.Sleep(2);
            if (cellToDraw.HasBottomWall())
            {
                int colPos = cellToDraw.Column * 2 + 1 + padding;
                int rowPos = cellToDraw.Row + padding + 1;
                Console.SetCursorPosition(colPos, rowPos);
                Console.Write("_");
            }
            else
            {
                int colPos = cellToDraw.Column * 2 + 1 + padding;
                int rowPos = cellToDraw.Row + padding + 1;
                Console.SetCursorPosition(colPos, rowPos);
                Console.Write(" ");
            }

            base.DrawRoom(cellToDraw);
        }
        public override void DrawMaze()
        {
            Cell current;
            for (int i = 0; i < GetWidth() * 2; i = i + 2)
                for (int j = 0; j < GetHeight(); j++)
                {
                    int k = i == 0 ? i : i / 2;

                    current = grid[k][j];
                    if (current.HasRightWall())
                    {
                        Console.SetCursorPosition(i + 2 + padding, j + padding + 1);
                        Console.Write("|");
                    }
                    else
                    {
                        if (j == GetHeight() - 1)
                        {
                            Console.SetCursorPosition(i + 2 + padding, j + padding + 1);
                            Console.Write("_");
                        }
                        else if (grid[k][j + 1].HasTopWall())
                        {
                            Console.SetCursorPosition(i + 2 + padding, j + padding + 1);
                            Console.Write("_");
                        }
                    }
                    //Thread.Sleep(2);
                    if (current.HasBottomWall())
                    {
                        Console.SetCursorPosition(i + 1 + padding, j + padding + 1);
                        Console.Write("_");
                    }
                }
            for (int i = 0; i < GetHeight(); i++)
            {
                Console.SetCursorPosition(0 + padding, i + 1 + padding);
                Console.Write("|");
            }

            for (int i = 0; i < GetWidth() * 2 - 1; i++)
            {
                Console.SetCursorPosition(i + 1 + padding, 0 + padding);
                Console.Write("_");
            }
        }
    }
}
