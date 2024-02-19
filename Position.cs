namespace Gem_Hunters
{
    public static class PositionState
    {
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

            public static bool IsPositionValid(int x, int y, int boardSize)
            {
                return x >= 0 && x < boardSize && y >= 0 && y < boardSize;
            }
        }
    }
}
