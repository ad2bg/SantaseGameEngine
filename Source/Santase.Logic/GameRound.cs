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
        private bool firstPlayerHasCollectedCards;
        private bool secondPlayerHasCollectedCards;

        private PlayerPosition firstToPlay;

        private BaseRoundState state;

        private PlayerPosition gameClosedBy;


        public int FirstPlayerPoints => this.firstPlayerPoints;

        public int SecondPlayerPoints => this.secondPlayerPoints;

        public bool FirstPlayerHasHand => this.firstPlayerHasCollectedCards;

        public bool SecondPlayerHasHand => this.secondPlayerHasCollectedCards;

        public PlayerPosition ClosedByPlayer => this.gameClosedBy;

        public PlayerPosition LastHandInPlayer => this.firstToPlay;

        public GameRound(IPlayer firstPlayer, IPlayer secondPlayer, PlayerPosition firstToPlay)
        {
            this.deck = new Deck();

            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;

            this.firstPlayerPoints = 0;
            this.secondPlayerPoints = 0;

            this.firstPlayerCards = new List<Card>();
            this.secondPlayerCards = new List<Card>();

            this.firstPlayerHasCollectedCards = false;
            this.secondPlayerHasCollectedCards = false;

            this.firstToPlay = firstToPlay;

            this.SetState(new StartRoundState(this));

            this.gameClosedBy = PlayerPosition.NoOne;
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
            IGameHand hand = new GameHand(
                this.firstToPlay,
                this.firstPlayer,
                this.firstPlayerCards,
                this.secondPlayer,
                this.secondPlayerCards,
                this.state,
                this.deck);

            hand.Start();

            // Update points
            this.UpdatePoints(hand);

            // Update collected cards
            if (hand.Winner == PlayerPosition.FirstPlayer)
            {
                firstPlayerHasCollectedCards=true;
            }
            else
            {
                secondPlayerHasCollectedCards=true;
            }

            // Draw new cards
            this.firstToPlay = hand.Winner;
            this.firstPlayerCards.Remove(hand.FirstPlayerCard);
            this.secondPlayerCards.Remove(hand.SecondPlayerCard);
            this.DrawNewCards();

            // Switch states as necessary
            this.state.PlayHand(this.deck.CardsLeft);

            // Close the game if needed
            if (hand.GameClosedBy == PlayerPosition.FirstPlayer ||
                hand.GameClosedBy == PlayerPosition.SecondPlayer)
            {
                this.gameClosedBy = hand.GameClosedBy;
            }
        }

        private void DrawNewCards()
        {
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
            if (hand.Winner == PlayerPosition.FirstPlayer)
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
