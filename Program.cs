using Gem_Hunters;
using static Gem_Hunters.Player_Initialization;
using static Gem_Hunters.PositionState;

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

    public Player_Initialization.Player Player1 { get; }
    public Player_Initialization.Player Player2 { get; }

    public Board()
    {
        grid = new Cell[6, 6];
        Player1 = new Player_Initialization.Player("P1", new Position(0, 0));
        Player2 = new Player_Initialization.Player("P2", new Position(5, 5));
        PlaceRandomGems();
        PlaceRandomObstacle();
        PlacePlayersOnTheBoard();

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

    private void PlacePlayersOnTheBoard()
    {
        // Place Player 1
        grid[Player1.Position.X, Player1.Position.Y] = new Cell("P1");

        // Place Player 2
        grid[Player2.Position.X, Player2.Position.Y] = new Cell("P2");
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

    public void displayBoard(Cell[,] grid)
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
    public bool IsValidMove(Player player, char direction)
    {
        int newXPosition = player.Position.X;
        int newYPosition = player.Position.Y;

        switch(direction)
        {
            case 'U':
                newYPosition--;
                break;
            case 'D':
                newYPosition++;
                break;
            case 'L':
                newXPosition--;
                break;
            case 'R':
                newXPosition++;
                break;
            default:
                Console.WriteLine("Invalid direction. Please enter U, D, L, or R.");
                return false;
        }

        if(newXPosition < 0 || newXPosition >= 6 || newYPosition < 0 || newYPosition >= 6)
        {
            Console.WriteLine("Out of a range. Choose different direction.");
            return false;
        }
        if (grid[newXPosition, newYPosition].Ocupant == "O")
        {
            Console.WriteLine("Can't move here. Road is blocked.");
            return false;
        }

        return true;
    }
}

public class Game
{
    private Board board;
    public Player_Initialization.Player Player1 { get; }
    public Player_Initialization.Player Player2 { get; }

    public Game()
    {
        board = new Board();
        Player1 = new Player_Initialization.Player("P1", new Position(0, 0));
        Player2 = new Player_Initialization.Player("P2", new Position(5, 5));
    }

    public void Start()
    {
        
        board.displayBoard(board.grid);
        int turns = 0;
        for (int i=0; turns < 30; i++)
        {
            //int currentTurn = 0;
            getTurn(turns);
            Console.WriteLine("Enter your Position: ");
            string userposition = Console.ReadLine().ToUpper();
            char direction = userposition[0];

            // Assuming turn 0 is for Player1 and turn 1 is for Player2
            if (turns % 2 == 0)
            {
                // Player1's turn
                Player1.Move(direction);
                board.displayBoard(board.grid);
            }
            else
            {
                // Player2's turn
                Player2.Move(direction);
                board.displayBoard(board.grid);
            }

            turns++;
        }
    }



    static void getTurn(int turn)
    {
        if (turn % 2 == 0)
        {
            Console.WriteLine("\nPlayer 1's turn: ");
        }
        else
        {
            Console.WriteLine("\nPlayer 2's turn: ");
        }
    }
}

class GemHunters
{
    static void Main(string[] args)
    {
        Game game = new Game();
        game.Start();
    }
}