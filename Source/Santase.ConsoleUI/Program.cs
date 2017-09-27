namespace Santase.ConsoleUI
{
    using System;
    using Santase.Logic;

    public static class Program
    {
        public static void Main()
        {
            ISantaseGame game = new SantaseGame(
                new ConsolePlayer(6,10),
                new ConsolePlayer(10,10));

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
