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

        public abstract PlayerAction GetTurn(PlayerTurnContext context);
    }
}
