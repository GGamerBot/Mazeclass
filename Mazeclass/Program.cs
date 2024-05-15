using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazeclass
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool[,] walls = new bool[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    walls[i, j] = false;
                }
            }
            Maze test = new Maze(walls, walls);
            test.MovePlayer(true, false);
            test.MovePlayer(false, false);
            for (int i = 0; i < 4; i++)
            {
                test.MovePlayer(true, true);
                test.MovePlayer(false, true);
                test.WriteFullStatus();
            }
            Console.ReadLine();
        }
    }
}
