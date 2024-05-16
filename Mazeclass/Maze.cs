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
         * 3*3 vertical walls
         * 4*2 horizontal walls
            0   1   2
    0 _ |0,0|1,0|2,0|3,0|
    1 _ |0,1|1,1|2,1|3,1|
        |0,2|1,2|2,2|3,2|
        */

        public Maze(bool[,] verticalWalls, bool[,] horizontalWalls)
        {
            //the edges are always walls

            //vert. walls. horizontal coord * vertical coord-1
            //horizontal walls: horizontal coord-1 * vertical walls
            this.height = verticalWalls.GetLength(0);  //GetLength(0), GetLength(1): gives me height and width of a matrix
            this.width = horizontalWalls.GetLength(1);
            if (verticalWalls.GetLength(0) == horizontalWalls.GetLength(0)+1 && verticalWalls.GetLength(1) == horizontalWalls.GetLength(1)+1)
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

        public bool AreWeWinning()
        {
            return cells[playerXCoord, playerYCoord] == MazeCellStatus.Goal;
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

            Console.WriteLine("Hortizontal walls: \n");
			for (int i = 0; i < height-1; i++)
			{
				for (int j = 0; j < width; j++)
				{
					Console.Write($"{horizontalWalls[i, j]} ");
				}
				Console.WriteLine("  horizontal");
			}

			Console.WriteLine("Vertical walls: \n");
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width-1; j++)
				{
					Console.Write($"{verticalWalls[i, j]} ");
				}
				Console.WriteLine("  horizontal");
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
                if (cells[playerXCoord, playerYCoord] == MazeCellStatus.Goal)
                {
                    Console.WriteLine("You won!");
                }
                else
                {

                cells[playerXCoord, playerYCoord] = MazeCellStatus.PlayerHere;
                }
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


        /*TODO PROC GEN
        //PROCEDURAL GENERATION
        //hunt-and-kill algorythm
        //I found it here: https://weblog.jamisbuck.org/2011/1/24/maze-generation-hunt-and-kill-algorithm
        //instead of adding nodes to a tree, I'm marking the cells as part of the maze, and breaking the walls where the edges would be
        //first a helper function for the random generator

        private List<Tuple<int, int>> FindUnaddedNeighbors(int xcoord, int ycoord)
        {
            List<Tuple<int, int>> unaddedNeighborList = new List<Tuple<int, int>>();

            if (xcoord + 1 < width && cells[xcoord + 1, playerYCoord] == MazeCellStatus.Buiding_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(playerXCoord + 1, playerYCoord));
            }
            if (xcoord - 1 <= 0 && cells[xcoord + 1, playerYCoord] == MazeCellStatus.Buiding_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(playerXCoord + 1, playerYCoord));
            }
            if (playerXCoord + 1 < height && cells[playerXCoord + 1, playerYCoord] == MazeCellStatus.Buiding_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(playerXCoord + 1, playerYCoord));
            }
            if (playerXCoord - 1 < height && cells[playerXCoord + 1, playerYCoord] == MazeCellStatus.Buiding_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(playerXCoord + 1, playerYCoord));
            }
            return unaddedNeighborList;
        }

        public Maze(int height, int width)
        {
            this.height = height;
            this.width = width;

            //gemerate initial, all-walls maze
            for (int i = 0; i < height - 1; i++)
            {
                for (int j = 0; j < width - 1; j++)
                {
                    verticalWalls[i, j] = true;
                    horizontalWalls[i, j] = true;
                }
            }
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    cells[i, j] = MazeCellStatus.Buiding_NotPartOfMaze;
                }
                Console.WriteLine();
            }
            //pseudo-player will walk through the maze
            //random start
            Random rnd = new Random();
            playerXCoord = rnd.Next(width);
            playerYCoord = rnd.Next(height);
            cells[playerXCoord, playerYCoord] = MazeCellStatus.Building_PartOfMaze;
            List<Tuple<int, int>> placesItCanGo = new List<Tuple<int, int>>();
            do
            {
                Tuple<int, int> nextCellCoords = placesItCanGo[rnd.Next(placesItCanGo.Count)];
                cells[playerXCoord, playerYCoord] = MazeCellStatus.Building_PartOfMaze;
                if (nextCellCoords.Item1 == playerXCoord) //they are on the same x coordinate => y coordinate must be different
                {
                    int wallYCoord = playerYCoord < nextCellCoords.Item2 ? playerYCoord : nextCellCoords.Item2;
                    horizontalWalls[playerXCoord, wallYCoord] = false;
                    //TODO: check if i break the right wall
                }
                else
                {
                    int wallXCoord = playerXCoord < nextCellCoords.Item1 ? playerXCoord : nextCellCoords.Item1;
                    horizontalWalls[wallXCoord, playerYCoord] = false;
                }
                playerXCoord = nextCellCoords.Item1;
                playerYCoord = nextCellCoords.Item2;

            } while (placesItCanGo.Count > 0);


        }*/
    }

}