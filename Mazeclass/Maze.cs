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
        Goal,
        Building_PartOfMaze,
        Building_NotPartOfMaze
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
            if (verticalWalls.GetLength(0) == horizontalWalls.GetLength(0)+1 && verticalWalls.GetLength(1)+1 == horizontalWalls.GetLength(1))
            {
				this.verticalWalls = verticalWalls;
				this.horizontalWalls = horizontalWalls;
				//only the inner walls are represented here
				playerXCoord = 0;
				playerYCoord = 0;
				cells = new MazeCellStatus[width, height];
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
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

			for (int i = 0; i < width; i++)
			{
                for (int j = 0; j < height; j++)
                {
                    Console.Write($"{i},{j} {cells[i, j]} ");
				}
                Console.WriteLine();
            }

            Console.WriteLine("Hortizontal walls: \n");
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height-1; j++)
				{
					Console.Write($"{horizontalWalls[i, j]} ");
				}
				Console.WriteLine("  horizontal");
			}

			Console.WriteLine("Vertical walls: \n");
			for (int i = 0; i < width-1; i++)
			{
				for (int j = 0; j < height; j++)
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


        //*TODO PROC GEN
        //PROCEDURAL GENERATION
        //hunt-and-kill algorythm
        //I found it here: https://weblog.jamisbuck.org/2011/1/24/maze-generation-hunt-and-kill-algorithm
        //instead of adding nodes to a tree, I'm marking the cells as part of the maze, and breaking the walls where the edges would be
        //first a helper function for the random generator

        private List<Tuple<int, int>> FindUnaddedNeighbors(int xcoord, int ycoord)
        {
            List<Tuple<int, int>> unaddedNeighborList = new List<Tuple<int, int>>();

            if (xcoord + 1 < width && cells[xcoord + 1, ycoord] == MazeCellStatus.Building_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(xcoord + 1, ycoord));
            }
            if (xcoord - 1 >= 0 && cells[xcoord - 1, ycoord] == MazeCellStatus.Building_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(xcoord - 1, ycoord));
            }
            
            if (ycoord + 1 < height && cells[xcoord, ycoord+1] == MazeCellStatus.Building_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(xcoord, ycoord+1));
            }
            
            if (ycoord - 1 >=0 && cells[xcoord, ycoord-1] == MazeCellStatus.Building_NotPartOfMaze)
            {
                unaddedNeighborList.Add(new Tuple<int, int>(xcoord, ycoord - 1));
            }
            return unaddedNeighborList;
        }

        public Maze(int width, int height)
        {
            this.height = height;
            this.width = width;
            this.horizontalWalls = new bool[width, height - 1];
            this.verticalWalls = new bool[width - 1,height];

            //vert. walls. horizontal coord * vertical coord-1
            //horizontal walls: horizontal coord-1 * vertical walls
            //generate initial, all-walls maze
            for (int i = 0; i < width - 1; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    verticalWalls[i, j] = true;
                    
                }
            }
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height - 1; j++)
                {
                    horizontalWalls[i, j] = true;
                }
            }
            cells = new MazeCellStatus[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    cells[i, j] = MazeCellStatus.Building_NotPartOfMaze;
                }
                Console.WriteLine();
            }
            //pseudo-player will walk through the maze
            //random start

            Random rnd = new Random();
            playerXCoord = rnd.Next(width);
            playerYCoord = rnd.Next(height);
            cells[playerXCoord, playerYCoord] = MazeCellStatus.Building_PartOfMaze;
            List<Tuple<int, int>> placesItCanGo = FindUnaddedNeighbors(playerXCoord,playerYCoord);
            do
            {
                //choose next place
                Tuple<int, int> nextCellCoords = placesItCanGo[rnd.Next(placesItCanGo.Count)];
                //break down wall
                if (playerXCoord == nextCellCoords.Item1)
                {
                    if (playerYCoord < nextCellCoords.Item2)
                    {
                        horizontalWalls[playerXCoord, playerYCoord] = false;
                    }
                    else
                    {
                        horizontalWalls[playerXCoord, nextCellCoords.Item2] = false;
                    }
                }
                else
                {
                    if (playerXCoord < nextCellCoords.Item1)
                    {
                        verticalWalls[playerXCoord, playerYCoord] = false;
                    }
                    else
                    {
                        horizontalWalls[nextCellCoords.Item1, playerYCoord] = false;
                    }
                }
                //go to next place
                playerXCoord = nextCellCoords.Item1;
                playerYCoord = nextCellCoords.Item2;
                //mark new place as maze part
                cells[playerXCoord, playerYCoord] = MazeCellStatus.Building_PartOfMaze;
                //find next possible places
                placesItCanGo = FindUnaddedNeighbors(playerXCoord, playerYCoord);
                WriteFullStatus();

            } while (placesItCanGo.Count > 0);


        }

    }
    

}