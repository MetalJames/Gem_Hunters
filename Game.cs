using static Gem_Hunters.Player_Initialization;
using static Gem_Hunters.PositionState;

namespace Gem_Hunters
{
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
            Console.WriteLine("Welcome to the \"Gem Hunters\"");
            Console.WriteLine("To play this game enter a direction (U, D, L, R).");
            Console.WriteLine();
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
            while (!IsGameOver(Player1, Player2, TotalTurns))
            {
                GetTurn(CurrentTurn, this);
                Console.WriteLine("Where do you want to go: ");
                string userposition = (Console.ReadLine() ?? "").ToUpper();

                if (!string.IsNullOrEmpty(userposition)) // we have to heck if input is not null or empty - as it crashes the game
                {
                    char direction = userposition[0];
                    if (board.IsValidMove(CurrentTurn, direction))
                    {
                        // Collecting gems
                        UpdateGameState(CurrentTurn, direction);
                        TotalTurns++;
                        SwitchTurns();
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a direction (U, D, L, R).");
                }
            }
            AnnounceWinner();
        }

        public void SwitchTurns()
        {
            CurrentTurn = (CurrentTurn == Player1) ? Player2 : Player1;
        }
        //In grid Y should go before X - just a reminder!
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

        private static void GetTurn(Player currentPlayer, Game game)
        {
            Console.WriteLine(currentPlayer == game.Player1 ? $"\n{game.P1Name}'s turn: " : $"\n{game.P2Name}'s turn: ");
        }

        public bool IsGameOver(Player Player1, Player Player2, int totalMoves)
        {
            int maxTurns = 30;
            int totalGems = 11;
            if (maxTurns <= totalMoves || Player1.GemCount >= totalGems || Player2.GemCount >= totalGems || board.CountRemainingGems() == 0)
            {
                Console.WriteLine();
                Console.WriteLine("Game Over!");
                return true;
            }
            return false;
        }

        public void AnnounceWinner()
        {
            Console.WriteLine();
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
            Console.WriteLine();
        }
    }
}