using Gem_Hunters;
using static Gem_Hunters.Player_Initialization;
using static Gem_Hunters.PositionState;

public enum OccupantType
{
    Empty,
    Player1,
    Player2,
    Gem,
    Obstacle
}

public class Cell
{
    public OccupantType Occupant { get; set; }

    public Cell(OccupantType occupant)
    {
        Occupant = occupant;
    }
}

public class Board
{
    public Cell[,] grid;

    public Player Player1 { get; }
    public Player Player2 { get; }

    public Board()
    {
        grid = new Cell[6, 6];
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        PlaceRandomGems();
        PlaceRandomObstacle();
        PlacePlayersOnTheBoard();
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (grid[i, j] == null)
                {
                    grid[i, j] = new Cell(OccupantType.Empty);
                }
            }
        }
    }

    private void PlacePlayersOnTheBoard()
    {
        // Place Player 1
        grid[Player1.Position.X, Player1.Position.Y] = new Cell(OccupantType.Player1);

        // Place Player 2
        grid[Player2.Position.X, Player2.Position.Y] = new Cell(OccupantType.Player2);
    }

    private void PlaceRandomGems()
    {
        Random random = new Random();
        int totalGems = 11;

        while (totalGems > 0)
        {
            int x = random.Next(0, 6);//random x position
            int y = random.Next(0, 6);//random y position

            //check if position for placing gem is empty
            if (grid[x, y] == null && !IsAdjacentToPlayer(x, y) && Position.IsPositionValid(x, y, grid.GetLength(0)))
            {
                grid[x, y] = new Cell(OccupantType.Gem);//we put our gem here
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
            if (grid[x, y] == null && !IsAdjacentToPlayer(x, y) && Position.IsPositionValid(x, y, grid.GetLength(0)))
            {
                grid[x, y] = new Cell(OccupantType.Obstacle);//we put our obstacle here
                totalObstacles--;
            }
        }
    }

    //I add this to prevent any player to be blocked by obstacle
    private bool IsAdjacentToPlayer(int x, int y)
    {
        // Check if the given position is adjacent to any player
        return (Math.Abs(x - Player1.Position.X) <= 1 && Math.Abs(y - Player1.Position.Y) <= 1) ||
               (Math.Abs(x - Player2.Position.X) <= 1 && Math.Abs(y - Player2.Position.Y) <= 1);
    }

    public void CollectGem(Player Player)
    {
        Player.GemCount++;
    }

    //counting remaining gems
    public int CountRemainingGems()
    {
        int count = 0;
        foreach(Cell cell in grid)
        {
            if(cell != null && cell.Occupant == OccupantType.Gem)
            {
                count++;
            }
        }
        return count;
    }

    public void DisplayBoard(Cell[,] grid, Player Player1, Player Player2, Game game)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            Console.WriteLine("");
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                switch (grid[i, j].Occupant) // Changed to switch statement - easier to read
                {
                    case OccupantType.Empty:
                        Console.Write("-  ");
                        break;
                    case OccupantType.Gem:
                        Console.Write("G  ");
                        break;
                    case OccupantType.Obstacle:
                        Console.Write("O  ");
                        break;
                    case OccupantType.Player1:
                        Console.Write("P1 ");
                        break;
                    case OccupantType.Player2:
                        Console.Write("P2 ");
                        break;
                }
            }
        }
        Console.WriteLine($"\n{game.P1Name} Gems: {Player1.GemCount}");
        Console.WriteLine($"{game.P2Name} Gems: {Player2.GemCount}");
        Console.WriteLine();
        Console.WriteLine($"Remaining Gems: {CountRemainingGems()}");
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
            Console.WriteLine("You can not fly. Choose different direction.");
            return false;
        }
        //same mistake as before Y should be before X in the grid
        //if (grid[newXPosition, newYPosition].Occupant == "O")
        if (grid[newYPosition, newXPosition].Occupant == OccupantType.Obstacle)
        {
            Console.WriteLine("Can't move here. Road is blocked.");
            return false;
        }
        //add this to prevent P1 going on top of P2
        if(grid[newYPosition, newXPosition].Occupant == OccupantType.Player1 || grid[newYPosition, newXPosition].Occupant == OccupantType.Player2)
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
    private Player Player1 { get; }
    private Player Player2 { get; }

    // Declare player names as fields
    public string P1Name { get; private set; }
    public string P2Name { get; private set; }

    public Player CurrentTurn { get; private set; }
    public int TotalTurns { get; private set; }

    public Game()
    {
        board = new Board();
        Player1 = new Player("P1", new Position(0, 0));
        Player2 = new Player("P2", new Position(5, 5));
        CurrentTurn = Player1;

        Console.Write("Enter Player 1's name: ");
        // Assign input to P1Name field
        string inputP1 = Console.ReadLine()!;
        P1Name = string.IsNullOrEmpty(inputP1) ? "Player 1" : inputP1;

        Console.Write("Enter Player 2's name: ");
        // Assign input to P2Name field
        string inputP2 = Console.ReadLine()!;
        P2Name = string.IsNullOrEmpty(inputP2) ? "Player 2" : inputP2;
    }
    public void Start()
    {
        board.DisplayBoard(board.grid, Player1, Player2, this);
        CurrentTurn = Player1;
        int TotalTurns = 0;
        while(!IsGameOver(Player1, Player2, TotalTurns)) 
        {
            GetTurn(CurrentTurn, this);
            Console.WriteLine("Enter your Position: ");
            string userposition = Console.ReadLine()!.ToUpper();
            char direction = userposition[0];

            if (board.IsValidMove(CurrentTurn, direction))
            {
                //Collecting gems
                UpdateGameState(CurrentTurn, direction);
                TotalTurns++;
                SwitchTurns();
            }
        }
        AnnounceWinner();
    }

    public void SwitchTurns()
    {
        CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
    }
    //In grid Y should go before X - so much time was wasted!
    //board.grid[currentPlayer.Position.X, currentPlayer.Position.Y].Occupant = "-";
    //have to create update game status method to make it more redable
    private void UpdateGameState(Player Player, char direction)
    {
        board.grid[Player.Position.Y, Player.Position.X].Occupant = OccupantType.Empty;
        Player.Move(direction);
        if (board.grid[Player.Position.Y, Player.Position.X].Occupant == OccupantType.Gem)
        {
            board.CollectGem(Player);
        }
        board.grid[Player.Position.Y, Player.Position.X].Occupant = Player == Player1 ? OccupantType.Player1 : OccupantType.Player2;
        board.DisplayBoard(board.grid, Player1, Player2, this);
    }

    //old method didnt work properly - so I have updated my code to display correct players turn
    //in case is player get to an obstacle or into another player - changed to ternary operator
    private static void GetTurn(Player currentPlayer, Game game)
    {
        Console.WriteLine(currentPlayer == game.Player1 ? $"\n{game.P1Name}'s turn: " : $"\n{game.P2Name}'s turn: ");
    }

    public bool IsGameOver(Player Player1, Player Player2, int totalMoves)
    {
        int maxTurns = 30;
        int totalGems = 11;
        if(maxTurns <= totalMoves || Player1.GemCount >= totalGems || Player2.GemCount >= totalGems || board.CountRemainingGems() == 0)
        {
            Console.WriteLine("Game Over!");
            return true;
        }
        return false;
    }

    public void AnnounceWinner()
    {
        if (Player1.GemCount > Player2.GemCount)
        {
            Console.WriteLine($"{P1Name} Wins!");
        }
        else if (Player1.GemCount < Player2.GemCount)
        {
            Console.WriteLine($"{P2Name} Wins!");
        }
        else
        {
            Console.WriteLine("It's a Tie!");
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