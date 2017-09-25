namespace Santase.ConsoleUI
{
    using System;
    using Santase.Logic.Cards;

    public static class Program
    {
        public static void Main()
        {
            var card = new Card(CardSuit.Heart, CardType.Ace);
            var card2 = new Card(CardSuit.Heart, CardType.Ace);
            Console.WriteLine(card == card2);


            Console.ReadLine();
        }
    }
}
