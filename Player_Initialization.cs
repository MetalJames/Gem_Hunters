using static Gem_Hunters.PositionState;

namespace Gem_Hunters
{
    public static class Player_Initialization
    {
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
    }
}
