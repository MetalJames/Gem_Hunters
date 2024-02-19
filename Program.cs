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

    public void displayBoard(Cell[,] grid, Player Player1, Player Player2)
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
                else if (grid[i, j].Ocupant == "P1")
                {
                    Console.Write("P1 ");
                }
                else if (grid[i, j].Ocupant == "P2")
                {
                    Console.Write("P2 ");
                }
                /*else
                {
                    Console.Write(grid[i, j].Ocupant + "  ");
                }*/
            }
        }
        Console.WriteLine($"\nPlayer 1 Gems: {Player1.GemCount}");
        Console.WriteLine($"Player 2 Gems: {Player2.GemCount}");
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
        //same mistake as before Y should be before X in the grid
        //if (grid[newXPosition, newYPosition].Ocupant == "O")
        if (grid[newYPosition, newXPosition].Ocupant == "O")
        {
            Console.WriteLine("Can't move here. Road is blocked.");
            return false;
        }
        //add this to prevent P1 going on top of P2
        if(grid[newYPosition, newXPosition].Ocupant == "P1" || grid[newYPosition, newXPosition].Ocupant == "P2")
        {
            Console.WriteLine("Ouch, Don't you see I'm here? Go elsewhere!");
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
        board.displayBoard(board.grid, Player1, Player2);
        Player currentPlayer = Player1;
        //have to try the other way to prevent wrong count
        int turns = 0;
        while(turns <= 30) 
        {
            getTurn(currentPlayer, this);
            Console.WriteLine("Enter your Position: ");
            string userposition = Console.ReadLine().ToUpper();
            char direction = userposition[0];

            if (board.IsValidMove(currentPlayer, direction))
            {
                //In grid Y should go before X - so much time was wasted!
                //board.grid[currentPlayer.Position.X, currentPlayer.Position.Y].Ocupant = "-";
                board.grid[currentPlayer.Position.Y, currentPlayer.Position.X].Ocupant = "-";
                currentPlayer.Move(direction);

                //Collecting gems
                if (board.grid[currentPlayer.Position.Y, currentPlayer.Position.X].Ocupant == "G")
                {
                    currentPlayer.CollectGem();
                    board.grid[currentPlayer.Position.Y, currentPlayer.Position.X].Ocupant = currentPlayer.Name;
                    board.displayBoard(board.grid, Player1, Player2);
                }
                else
                {
                    board.grid[currentPlayer.Position.Y, currentPlayer.Position.X].Ocupant = currentPlayer.Name;
                    board.displayBoard(board.grid, Player1, Player2);
                }
                turns++;
                SwitchTurs(ref currentPlayer);
            }
        }
    }

    public void SwitchTurs(ref Player currentPlayer)
    {
        currentPlayer = (currentPlayer == Player1) ? Player2 : Player1;
    }

    //old method didnt work properly - so I have updated my code to display correct players turn
    //in case is player get to an obstacle or into another player
    static void getTurn(Player currentPlayer, Game game)
    {
        if (currentPlayer == game.Player1)
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