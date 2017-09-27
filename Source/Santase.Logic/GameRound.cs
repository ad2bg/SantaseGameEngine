namespace Santase.Logic
{
    using System;
    using System.Collections.Generic;
    using Santase.Logic.Cards;
    using Santase.Logic.Cards.Contracts;
    using Santase.Logic.Players;

    public class GameRound : IGameRound
    {
        private IDeck deck;
        private IPlayer firstPlayer;
        private IPlayer secondPlayer;
        private int firstPlayerPoints;
        private int secondPlayerPoints;
        private IList<Card> firstPlayerCards;
        private IList<Card> secondPlayerCards;
        private IList<Card> firstPlayerCollectedCards;
        private IList<Card> secondPlayerCollectedCards;

        private PlayerPosition whoWillPlayFirst;


        public int FirstPlayerPoints => this.firstPlayerPoints;

        public int SecondPlayerPoints => this.secondPlayerPoints;

        public bool FirstPlayerHasHand => this.firstPlayerCollectedCards.Count > 0;

        public bool SecondPlayerHasHand => this.secondPlayerCollectedCards.Count > 0;

        public PlayerPosition ClosedByPlayer => throw new NotImplementedException();

        public GameRound(IPlayer firstPlayer, IPlayer secondPlayer, PlayerPosition whoWillPlayFirst)
        {
            this.deck = new Deck();
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.firstPlayerPoints = 0;
            this.secondPlayerPoints = 0;
            this.firstPlayerCards = new List<Card>();
            this.secondPlayerCards = new List<Card>();
            this.firstPlayerCollectedCards = new List<Card>();
            this.secondPlayerCollectedCards = new List<Card>();
        }


        public void Start()
        {
            this.DealFirstCards();
            while (!this.IsFinished())
            {
                this.PlayHand();
            }
        }

        private void PlayHand()
        {
            IGameHand hand = new GameHand();
            hand.Start();
            // TODO: Update points
            // TODO: Add one more card to both players
            // TODO: Update collected cards
        }

        private void DealFirstCards()
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    var card = this.deck.GetNextCard();
                    this.firstPlayer.AddCard(card);
                }
                for (int i = 0; i < 3; i++)
                {
                    var card = this.deck.GetNextCard();
                    this.secondPlayer.AddCard(card);
                }
            }
        }


        private bool IsFinished()
        {
            return
                this.firstPlayerPoints >= 66 ||
                this.secondPlayerPoints >= 66 ||
                this.firstPlayerCards.Count == 0 ||
                this.secondPlayerCards.Count == 0;
        }


    }
}
