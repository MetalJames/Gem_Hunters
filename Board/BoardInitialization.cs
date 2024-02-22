using static Gem_Hunters.PositionState;

namespace Gem_Hunters
{
    public partial class Board
    {
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
                    grid[x, y] = new Cell(OccupantType.Gem);//we put our gems here
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
                    grid[x, y] = new Cell(OccupantType.Obstacle);//we put our obstacles here
                    totalObstacles--;
                }
            }
        }

        private bool IsAdjacentToPlayer(int x, int y)
        {
            // Check if the given position is adjacent to any player
            return (Math.Abs(x - Player1.Position.X) <= 1 && Math.Abs(y - Player1.Position.Y) <= 1) ||
                   (Math.Abs(x - Player2.Position.X) <= 1 && Math.Abs(y - Player2.Position.Y) <= 1);
        }
    }
}