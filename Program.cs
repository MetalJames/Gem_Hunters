using System;

public class Position
{
    public int X { get; }
    public int Y { get; }

    // Position Constructor
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Player
{
    public string Name { get; }
    public Position Position { get; private set; }
    public int GemCount { get; private set; }

    // Player Constructor
    public Player(string name, Position position)
    {
        Name = name;
        Position = position;
        GemCount = 0;
    }

    // Method to update player's position based on the input direction (U, D, L, R)
    public void Move(char direction)
    {
        switch (direction)
        {
            case 'U':
                Position = new Position(Position.X, Position.Y - 1);
                break;
            case 'D':
                Position = new Position(Position.X, Position.Y + 1);
                break;
            case 'L':
                Position = new Position(Position.X - 1, Position.Y);
                break;
            case 'R':
                Position = new Position(Position.X + 1, Position.Y);
                break;
            default:
                break;
        }
    }
}

public class Cell
{
    public string Ocupant { get; set; }

    public Cell(string ocupant)
    {
        Ocupant = ocupant;
    }
}

public class Board
{
    public Cell[,] grid;

    public Board()
    {
        grid = new Cell[6, 6];
        PlaceRandomGems();
        PlaceRandomObstacle();

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (grid[i, j] == null)
                {
                    grid[i, j] = new Cell("-");
                }
            }
        }
    }

    private void PlaceRandomGems()
    {
        Random random = new Random();
        int totalGems = 10;

        while (totalGems > 0)
        {
            int x = random.Next(0, 6);//random x position
            int y = random.Next(0, 6);//random y position

            //check if position for placing gem is empty
            if (grid[x, y] == null)
            {
                grid[x, y] = new Cell("G");//we put our gem here
                totalGems--;
            }
        }
    }

    private void PlaceRandomObstacle()
    {
        Random random = new Random();
        int totalObstacles = 6;

        while (totalObstacles > 0)
        {
            int x = random.Next(0, 6);//random x position
            int y = random.Next(0, 6);//random y position

            //check if position for placing obstacle is empty
            if (grid[x, y] == null)
            {
                grid[x, y] = new Cell("O");//we put our obstacle here
                totalObstacles--;
            }
        }
    }
}

public class Game
{
    static void Main(string[] args)
    {
        Console.WriteLine(new Player("John", new Position(3, 4)));

        Board board = new Board();

        displayBoard(board.grid);
    }

    static void displayBoard(Cell[,] grid)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            Console.WriteLine("");
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].Ocupant == "-")
                {
                    Console.Write("-  ");
                }
                else if (grid[i, j].Ocupant == "G")
                {
                    Console.Write("G  ");
                }
                else if (grid[i, j].Ocupant == "O")
                {
                    Console.Write("O  ");
                }
                else
                {
                    Console.Write(grid[i, j].Ocupant + "  ");
                }
            }
        }
    }
}

