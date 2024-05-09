using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazeclass
{
    internal class Maze
    {
        private int height { get; set; }
        private int width { get; set; }
        public int[,] cells { get; set; }
        //true = there is a wall there. false: there isn't a wall there.
        public bool[,] verticalWalls { get; set; }
        public bool[,] horizontalWalls { get; set; }
        public int playerXCoord { get; set; }
        public int playerYCoord { get; set; }
        //first coordinate is horizontal, X, second is vertical, Y
        //they start at the upper left corner
        /*
         *  0   1   2
    0   |0,0|1,0|2,0|3,0|
    1   |0,1|1,1|2,1|3,1|
    2   |0,2|1,2|2,2|3,2|
        */

        public Maze(bool[,] verticalWalls, bool[,] horizontalWalls)
        {
            //requirement: the size of the two matrixes need to be the same
            //TODO: throw an error if it's invalid
            this.verticalWalls = verticalWalls;
            this.horizontalWalls = horizontalWalls;
            //only the inner walls are represented here
            //the edges are always walls
            this.height = verticalWalls.GetLength(0)+1;
            this.width = verticalWalls.GetLength(1)+1;
            playerXCoord = 0;
            playerYCoord = 0;
        }
        public bool CanIGoThere(bool areWeGoingVertical, bool areWeIncreasingCoord)
        {
            int wallXCoord = this.playerXCoord;
            int wallYCoord = this.playerXCoord;
            //these ^ are unnecessary; i need to clean up the if-elses
            if (areWeGoingVertical)
            {
                wallYCoord = areWeIncreasingCoord ? playerYCoord : playerYCoord - 1;
            }
            else
            {
                wallXCoord = areWeIncreasingCoord ? playerXCoord:playerXCoord-1;                
            }
            bool isOutOfBounds = wallXCoord < 0 || wallYCoord < 0 || wallXCoord == horizontalWalls.Length || wallYCoord == verticalWalls.Length;
            if (isOutOfBounds)
            {
                return false;
            }
            else
            {
                return !(areWeGoingVertical ? horizontalWalls[wallXCoord, wallYCoord] : verticalWalls[wallXCoord, wallYCoord]);
            }
        }
    }

}
