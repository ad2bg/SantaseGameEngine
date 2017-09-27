namespace Santase.ConsoleUI
{
    using System;
    using System.Threading;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        int row;
        int col;

        public ConsolePlayer(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override void AddCard(Card card)
        {
            base.AddCard(card);

            Console.SetCursorPosition(this.col, this.row);
            foreach (var item in this.cards)
            {
                Console.Write("{0} ", item.ToString());
            }
            Thread.Sleep(150);
        }

        public override PlayerAction GetTurn(
            PlayerTurnContext context,
            IPlayerActionValidator actionValidator)
        {
            this.PrintGameInfo(context);

            while (true)
            {
                PlayerAction playerAction = null;

                Console.SetCursorPosition(0, this.row + 1);
                Console.Write(
                    "Turn? [1-{0}]=Card{1} : ",
                    this.cards.Count,
                    context.AmItheFirstPlayer ? 
                    ", [T]=Change Trump; [C]=Close" : "");

                var userActionAsString = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userActionAsString))
                {
                    Console.WriteLine("Invalid entry!          ");
                    continue;
                }

                if (userActionAsString[0] >= '1' &&
                    userActionAsString[0] <= '6')
                {

                    var cardIndex =
                        int.Parse(userActionAsString[0].ToString()) - 1;

                    if (cardIndex >= this.cards.Count)
                    {
                        Console.WriteLine("Invalid card index!          ");
                        continue;
                    }

                    var card = this.cards[cardIndex];

                    var possibleAnnouce = Announce.None;

                    if (context.AmItheFirstPlayer)
                    {
                        possibleAnnouce =
                            this.PossibleAnnounce(card, context.TrumpCard);

                        if (possibleAnnouce != Announce.None)
                        {


                            // Ask the user whether to announce it
                            while (true)
                            {
                                Console.SetCursorPosition(0, this.row + 2);
                                Console.Write(
                                    "Announce {0} [Y]/[N]:     ", 
                                    possibleAnnouce.ToString());

                                var userInput = Console.ReadLine();

                                if (string.IsNullOrWhiteSpace(userInput))
                                {
                                    Console.WriteLine(
                                        "Please enter [Y] or [N]!     ");
                                    continue;
                                }

                                if (userInput[0] == 'N')
                                {
                                    possibleAnnouce = Announce.None;
                                    break;
                                }
                                else if (userInput[0] == 'Y')
                                {
                                    break;
                                }
                            }
                        }

                    }

                    playerAction =
                        new PlayerAction(
                            PlayerActionType.PlayCard,
                            card,
                            possibleAnnouce);

                }
                else if (userActionAsString[0] >= 'T')
                {
                    playerAction =
                        new PlayerAction(
                            PlayerActionType.ChangeTrump,
                            null,
                            Announce.None);
                }
                else if (userActionAsString[0] >= 'C')
                {
                    playerAction =
                        new PlayerAction(
                            PlayerActionType.CloseGame,
                            null,
                            Announce.None);
                }
                else
                {
                    Console.WriteLine("Invalid entry!          ");
                    continue;
                }

                if (actionValidator.IsValid(playerAction, context))
                {
                    this.PrintGameInfo(context);
                    return playerAction;
                }
                else
                {
                    Console.WriteLine("Invalid action!          ");
                    continue;
                }
            }
        }

        private void PrintGameInfo(PlayerTurnContext context)
        {
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Trump card: {0}   ", 
                context.TrumpCard);

            Console.SetCursorPosition(0, 1);
            Console.WriteLine("Cards left in deck: {0}  ", 
                context.CardsleftInDeck);

            Console.SetCursorPosition(0, 2);
            Console.WriteLine("Board: {0}{1}  ", 
                context.FirstPlayedCard,
                context.SecondPlayedCard);

            Console.SetCursorPosition(0, 3);
            Console.WriteLine("Game state: {0}             ",
                context.State.GetType().Name);


        }
    }
}
