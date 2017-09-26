namespace Santase.ConsoleUI
{
    using System;
    using Santase.Logic;

    public static class Program
    {
        public static void Main()
        {
            ISantaseGame game = new SantaseGame();
            game.Start();


            Console.WriteLine("Game over!");
            Console.WriteLine("{0} - {1}",
                game.FirstPlayerTotalPoints,
                game.SecondPlayerTotalPoints);
            Console.ReadLine();
        }
    }
}
