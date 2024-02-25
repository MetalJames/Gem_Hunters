using static Gem_Hunters.Player_Initialization;
using static Gem_Hunters.PositionState;

namespace Gem_Hunters
{
    public partial class Board
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
            Console.WriteLine("");
            Console.WriteLine($"\n{game.P1Name} Gems: {Player1.GemCount}");
            Console.WriteLine($"{game.P2Name} Gems: {Player2.GemCount}");
            Console.WriteLine();
            Console.WriteLine($"Remaining Gems: {CountRemainingGems()}");
            int MaxMoves = 30;
            int remainingMoves = MaxMoves - game.TotalTurns;
            Console.WriteLine($"Remaining Moves: {remainingMoves}");
        }
        public bool IsValidMove(Player player, char direction)
        {
            int newXPosition = player.Position.X;
            int newYPosition = player.Position.Y;

            switch (direction)
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

            if (newXPosition < 0 || newXPosition >= 6 || newYPosition < 0 || newYPosition >= 6)
            {
                Console.WriteLine("You can not fly. Choose different direction.");
                return false;
            }
            if (grid[newYPosition, newXPosition].Occupant == OccupantType.Obstacle)
            {
                Console.WriteLine("Can't move here. Road is blocked.");
                return false;
            }
            //add this to prevent P1 going on top of P2
            if (grid[newYPosition, newXPosition].Occupant == OccupantType.Player1 || grid[newYPosition, newXPosition].Occupant == OccupantType.Player2)
            {
                Console.WriteLine("Ouch, Don't you see I'm here? Go elsewhere!");
                return false;
            }
            return true;
        }

        public void CollectGem(Player Player)
        {
            Player.GemCount++;
        }

        //counting remaining gems
        public int CountRemainingGems()
        {
            int count = 0;
            foreach (Cell cell in grid)
            {
                if (cell != null && cell.Occupant == OccupantType.Gem)
                {
                    count++;
                }
            }
            return count;
        }
    }
}