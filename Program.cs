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
}

public class Game
{
    static void Main(string[] args)
    {
        Console.WriteLine(new Player("John", new Position(3, 4)));
    }
}

