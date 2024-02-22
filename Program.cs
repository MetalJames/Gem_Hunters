using Gem_Hunters;

class GemHunters
{
    static void Main(string[] args)
    {
        bool playAgain = true;

        while (playAgain)
        {
            Game game = new Game();
            game.Start();

            bool validInput = false;
            while (!validInput)
            {
                Console.WriteLine("Do you want to play again? (Yes/No)");

                string userInput = Console.ReadLine()?.Trim().ToUpper() ?? "";

                if (userInput == "YES" || userInput == "Y")
                {
                    validInput = true;
                    playAgain = true;
                }
                else if (userInput == "NO" || userInput == "N")
                {
                    validInput = true;
                    playAgain = false;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter either 'Yes' or 'No'.");
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine("Exiting Gem Hunters. Goodbye!");
        Console.WriteLine();
    }
}