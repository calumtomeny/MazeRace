using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Models
{
    [Serializable]
    public class Cell
    {
        public bool[] walls;
        public int Row { get; set; }
        public int Column { get; set; }

        public Cell(int column,int row)
        {
            Row = row;
            Column = column;
            walls = new bool[4];
            for (int i = 0; i < walls.Length; i++) { walls[i] = true; }
        }

        public bool HasAllWalls()
        {
            return walls.Where(x => x == true).Count() == 4;
        }

        public bool HasTopWall()
        {
            return walls[0];
        }

        public bool HasRightWall()
        {
            return walls[1];
        }

        public bool HasBottomWall()
        {
            return walls[2];
        }

        public bool HasLeftWall()
        {
            return walls[3];
        }

        public void KnockDownWall(Wall wallToKnockDown)
        {
            switch (wallToKnockDown)
            {
                case Wall.Top:
                    walls[0] = false;
                    break;
                case Wall.Right:
                    walls[1] = false;
                    break;
                case Wall.Bottom:
                    walls[2] = false;
                    break;
                case Wall.Left:
                    walls[3] = false;
                    break;
            }
        }
    }
}
