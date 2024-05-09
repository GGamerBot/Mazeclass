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
            Console.WriteLine("no syntax error");
            bool[,] walls = new bool[3,3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    walls[i,j] = false;
                }
            }
            Maze test = new Maze(walls, walls);
            //horizontal backward:
            Console.WriteLine(test.CanIGoThere(true, false));

            Console.ReadLine();
        }
    }
}
