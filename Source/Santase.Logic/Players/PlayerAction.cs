namespace Santase.Logic.Players
{
    using Santase.Logic.Cards;

    public class PlayerAction
    {
        public PlayerAction(
            PlayerActionType type, 
            Card card,
            Announce announce)
        {
            this.Type = type;
            this.Card = card;
            this.Announce = announce;
        }

        public Card Card { get; private set; }

        public PlayerActionType Type { get; private set; }

        public Announce Announce { get; private set; }
    }
}