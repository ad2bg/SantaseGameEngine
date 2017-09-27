namespace Santase.Logic
{
    using System;
    using System.Collections.Generic;
    using Santase.Logic.Cards;
    using Santase.Logic.Cards.Contracts;
    using Santase.Logic.Players;
    using Santase.Logic.RoundStates;

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

        private PlayerPosition firstToPlay;

        private BaseRoundState state;


        public int FirstPlayerPoints => this.firstPlayerPoints;

        public int SecondPlayerPoints => this.secondPlayerPoints;

        public bool FirstPlayerHasHand => this.firstPlayerCollectedCards.Count > 0;

        public bool SecondPlayerHasHand => this.secondPlayerCollectedCards.Count > 0;

        public PlayerPosition ClosedByPlayer => throw new NotImplementedException();

        public GameRound(IPlayer firstPlayer, IPlayer secondPlayer, PlayerPosition firstToPlay)
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

            this.firstToPlay = firstToPlay;

            this.SetState(new StartRoundState(this));
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
            this.UpdatePoints(hand);
            
            // TODO: Last 10


            // Update collected cards
            if (hand.Winner==PlayerPosition.FirstPlayer)
            {
                firstPlayerCollectedCards.Add(hand.FirstPlayerCard);
                firstPlayerCollectedCards.Add(hand.SecondPlayerCard);
            }
            else
            {
                secondPlayerCollectedCards.Add(hand.FirstPlayerCard);
                secondPlayerCollectedCards.Add(hand.SecondPlayerCard);
            }

            // Draw new cards
            this.firstToPlay = hand.Winner;

            if (this.state.ShouldDrawCard)
            {
                if (this.firstToPlay == PlayerPosition.FirstPlayer)
                {
                    this.GiveCardToFirstPlayer();
                    this.GiveCardToSecondPlayer();
                }
                else
                {
                    this.GiveCardToSecondPlayer();
                    this.GiveCardToFirstPlayer();
                }
            }
        }

        private void UpdatePoints(IGameHand hand)
        {
            if (hand.Winner==PlayerPosition.FirstPlayer)
            {
                this.firstPlayerPoints += hand.FirstPlayerCard.GetValue();
                this.firstPlayerPoints += hand.SecondPlayerCard.GetValue();
            }
            else
            {
                this.secondPlayerPoints += hand.FirstPlayerCard.GetValue();
                this.secondPlayerPoints += hand.SecondPlayerCard.GetValue();
            }

            this.firstPlayerPoints += (int)hand.FirstPlayerAnnounce;
            this.secondPlayerPoints += (int)hand.SecondPlayerAnnounce;
        }

        private void GiveCardToFirstPlayer()
        {
            var card = this.deck.GetNextCard();
            this.firstPlayer.AddCard(card);
            this.firstPlayerCards.Add(card);
        }
        private void GiveCardToSecondPlayer()
        {
            var card = this.deck.GetNextCard();
            this.secondPlayer.AddCard(card);
            this.secondPlayerCards.Add(card);
        }

        private void DealFirstCards()
        {
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 3; i++) { this.GiveCardToFirstPlayer(); }
                for (int i = 0; i < 3; i++) { this.GiveCardToSecondPlayer(); }
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

        public void SetState(BaseRoundState newState)
        {
            this.state = newState;
        }
    }
}
