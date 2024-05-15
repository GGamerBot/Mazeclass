using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace Mazeclass
{

    enum MazeCellStatus
    {
        Unvisited,
        Visited,
        PlayerHere,
        Goal
    }
    internal class Maze
    { 
        //technically i could get height and width from the matrixes
        //but it would make things a lot more complicated
        private int height { get; set; }
        private int width { get; set; }
        //cells matrix will be used to mark the cells: goal + currently standing on + snail trail we made
        public MazeCellStatus[,] cells { get; set; }
        //true = there is a wall there. false: there isn't a wall there.
        public bool[,] verticalWalls { get; set; }
        public bool[,] horizontalWalls { get; set; }
        public int playerXCoord { get; set; }
        public int playerYCoord { get; set; }

        //first coordinate is horizontal, X, second is vertical, Y
        //they start at the upper left corner
        /*
         * width = 4, height = 3
            0   1   2
    0 _ |0,0|1,0|2,0|3,0|
    1 _ |0,1|1,1|2,1|3,1|
        |0,2|1,2|2,2|3,2|
        */

        public Maze(bool[,] verticalWalls, bool[,] horizontalWalls)
        {
            //the edges are always walls
            this.height = verticalWalls.GetLength(0)+1;  //GetLength(0), GetLength(1): gives me height and width of a matrix
            this.width = verticalWalls.GetLength(1)+1;
            if (verticalWalls.GetLength(0) == horizontalWalls.GetLength(0) && verticalWalls.GetLength(1) == horizontalWalls.GetLength(1))
            {
				this.verticalWalls = verticalWalls;
				this.horizontalWalls = horizontalWalls;
				//only the inner walls are represented here
				playerXCoord = 0;
				playerYCoord = 0;
				cells = new MazeCellStatus[height, width];
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        cells[i, j] = MazeCellStatus.Unvisited;
                    }
                }
                cells[0, 0] = MazeCellStatus.PlayerHere;
                cells[width - 1, height - 1] = MazeCellStatus.Goal;
            }
            else
            {
                throw new ArgumentOutOfRangeException(":(");
            }


        }

        public void WriteFullStatus() //FOR DEBUGGING. writes everything on the console.
        {
            Console.WriteLine("Cells: \n");

			for (int i = 0; i < height; i++)
			{
                for (int j = 0; j < width; j++)
                {
                    Console.Write($"{cells[i, j]} ");
				}
                Console.WriteLine();
            }

            Console.WriteLine("Walls: \n");
			for (int i = 0; i < height-1; i++)
			{
				for (int j = 0; j < width-1; j++)
				{
					Console.Write($"{horizontalWalls[i, j]} ");
				}
				Console.WriteLine("  horizontal");

				for (int j = 0; j < width-1; j++)
				{
					Console.Write($"{verticalWalls[i, j]} ");
				}
				Console.WriteLine("  vertical");
			}
            Console.WriteLine($"player coords: {playerXCoord}, {playerYCoord}");

        }
        //END OF DEBUG

        //using these two bools (areWeGoingVertical,areWeIncreasingCoord)
        //makes this feel more understandable for me
        //feel free to revise. maybe keystroke would work better
        public void MovePlayer(bool areWeGoingVertical, bool areWeIncreasingCoord)
        {
            if (CanIGoThere(areWeGoingVertical, areWeIncreasingCoord))
            {
                cells[playerXCoord, playerYCoord] = MazeCellStatus.Visited;
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
                cells[playerXCoord, playerYCoord] = MazeCellStatus.PlayerHere;
            }

		}

        private bool CanIGoThere(bool areWeGoingVertical, bool areWeIncreasingCoord)
        {
            int wallXCoord = this.playerXCoord;
            int wallYCoord = this.playerXCoord;
            if (areWeGoingVertical)
            {
                wallYCoord = areWeIncreasingCoord ? playerYCoord : playerYCoord - 1;
            }
            else
            {
                wallXCoord = areWeIncreasingCoord ? playerXCoord:playerXCoord-1;                
            }
            //is out of bounds: as in, are we trying to go out of bounds
            //if the wall coords are outside the wall matrix, then it means they are the outer edges not represented in the matrixes
            //which are always walls, never walkable
            bool isOutOfBounds = wallXCoord < 0 || wallYCoord < 0 || wallXCoord == width-1 || wallYCoord == height-1; 
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
