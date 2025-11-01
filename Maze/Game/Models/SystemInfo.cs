using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Game.Models
{
    public static class SystemInfo
    {
        public static string MachineName
        {
            get
            {
                return System.Environment.MachineName;
            }
        }
        public static string UserName
        {
            get
            {
                return System.Environment.UserName;
            }
        }
        public static string OSVersion
        {
            get
            {
                return System.Environment.OSVersion.ToString();
            }
        }
    }
}

