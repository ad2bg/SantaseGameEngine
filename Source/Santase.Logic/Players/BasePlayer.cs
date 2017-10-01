namespace Santase.Logic.Players
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Santase.Logic.Cards;

    public abstract class BasePlayer : IPlayer
    {
        protected IList<Card> cards;

        protected BasePlayer()
        {
            this.cards = new List<Card>();
        }

        public virtual void AddCard(Card card)
        {
            this.cards.Add(card);
        }

        public abstract PlayerAction GetTurn(
            PlayerTurnContext context,
            IPlayerActionValidator actionValidator);

        public virtual void EndTurn(PlayerTurnContext context)
        {
        }

        protected Announce PossibleAnnounce(Card cardToBePlayed, Card trumpCard)
        {
            CardType cardType = cardToBePlayed.Type;
            CardType cardTypeToSearch;

            if (cardType == CardType.Queen)
            {
                cardTypeToSearch = CardType.King;
            }
            else if (cardType == CardType.King)
            {
                cardTypeToSearch = CardType.Queen;
            }
            else
            {
                return Announce.None;
            }

            Card cardToSearch = new Card(cardToBePlayed.Suit, cardTypeToSearch);

            if (!this.cards.Contains(cardToSearch))
            {
                return Announce.None;
            }

            if (cardToBePlayed.Suit==trumpCard.Suit)
            {
                return Announce.Fourty;
            }
            else
            {
                return Announce.Twenty;
            }
        }

    }
}
