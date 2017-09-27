namespace Santase.ConsoleUI
{
    using System;
    using Santase.Logic;

    public static class Program
    {
        public static void Main()
        {
            Console.BufferHeight = Console.WindowHeight = 16;
            Console.BufferWidth = Console.WindowWidth = 60;

            ISantaseGame game = new SantaseGame(
                new ConsolePlayer(5, 10),
                new ConsolePlayer(10, 10),
                PlayerPosition.FirstPlayer);

            game.Start();


            Console.WriteLine("Game over!");
            Console.WriteLine("{0} - {1} in {2} rounds",
                game.FirstPlayerTotalPoints,
                game.SecondPlayerTotalPoints,
                game.RoundsPlayed);
            Console.ReadLine();
        }
    }
}
