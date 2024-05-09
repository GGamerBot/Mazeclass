using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazeclass
{
    internal class Maze
    { 
        //technically i could get height and width from the matrixes
        //but it would make things a lot more complicated
        private int height { get; set; }
        private int width { get; set; }
        //cells matrix will be used to mark the cells: goal + currently standing on + snail trail we made
        public int[,] cells { get; set; }
        //true = there is a wall there. false: there isn't a wall there.
        public bool[,] verticalWalls { get; set; }
        public bool[,] horizontalWalls { get; set; }
        public int playerXCoord { get; set; }
        public int playerYCoord { get; set; }

        //first coordinate is horizontal, X, second is vertical, Y
        //they start at the upper left corner
        /*
            0   1   2
    0 _ |0,0|1,0|2,0|3,0|
    1 _ |0,1|1,1|2,1|3,1|
        |0,2|1,2|2,2|3,2|
        */

        public Maze(bool[,] verticalWalls, bool[,] horizontalWalls)
        {
            //requirement: the size of the two matrixes need to be the same
            //TODO: throw an error if it's invalid
            this.verticalWalls = verticalWalls;
            this.horizontalWalls = horizontalWalls;
            //only the inner walls are represented here
            //the edges are always walls
            this.height = verticalWalls.GetLength(0);
            this.width = verticalWalls.GetLength(1);
            playerXCoord = 0;
            playerYCoord = 0;
        }
        //using these two bools (areWeGoingVertical,areWeIncreasingCoord)
        //makes this feel more understandable for me
        //feel free to revise. maybe keystroke would work better
        public void MovePlayer(bool areWeGoingVertical, bool areWeIncreasingCoord)
        {
            if (CanIGoThere(areWeGoingVertical, areWeIncreasingCoord))
            {
                if (areWeGoingVertical && areWeIncreasingCoord)
                {
                    playerYCoord++;
                }
                else if (areWeGoingVertical)
                {
                    playerYCoord--;
                }
                else if (areWeIncreasingCoord)
                {
                    playerXCoord++;
                }
                else
                {
                    playerXCoord--;
                }
            }
            //I don't know how to make a better system to show me while debugging
            //because. i only have a console
            //wpf should be implemented later
                Console.WriteLine($"{playerXCoord}, {playerYCoord}");
        }

        private bool CanIGoThere(bool areWeGoingVertical, bool areWeIncreasingCoord)
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
            bool isOutOfBounds = wallXCoord < 0 || wallYCoord < 0 || wallXCoord == width || wallYCoord == height;
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
