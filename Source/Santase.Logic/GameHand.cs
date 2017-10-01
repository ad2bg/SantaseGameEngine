
namespace Santase.Logic
{
    using System.Collections;
    using System.Collections.Generic;

    using Santase.Logic.Cards;
    using Santase.Logic.Cards.Contracts;
    using Santase.Logic.Players;
    using Santase.Logic.RoundStates;

    public class GameHand : IGameHand
    {
        private PlayerPosition whoWillPlayFirst;
        private IPlayer firstPlayer;
        private IList<Card> firstPlayerCards;
        private IPlayer secondPlayer;
        private IList<Card> secondPlayerCards;
        private BaseRoundState state;
        private IDeck deck;
        private IPlayerActionValidator actionValidator;
        private PlayerPosition whoClosedTheGame;
        private Card firstPlayerCard;
        private Card secondPlayerCard;
        private PlayerPosition winner;

        private Announce firstPlayerAnnounce;
        private Announce secondPlayerAnnounce;

        public GameHand(
            PlayerPosition whoWillPlayFirst,
            IPlayer firstPlayer,
            IList<Card> firstPlayerCards,
            IPlayer secondPlayer,
            IList<Card> secondPlayerCards,
            BaseRoundState state,
            IDeck deck)
        {
            this.whoWillPlayFirst = whoWillPlayFirst;
            this.firstPlayer = firstPlayer;
            this.firstPlayerCards = firstPlayerCards;
            this.secondPlayer = secondPlayer;
            this.secondPlayerCards = secondPlayerCards;
            this.state = state;
            this.deck = deck;
            this.actionValidator = new PlayerActionValidator();
            this.whoClosedTheGame = PlayerPosition.NoOne;
        }

        public PlayerPosition Winner => this.winner;

        public Card FirstPlayerCard => this.firstPlayerCard;

        public Card SecondPlayerCard => this.secondPlayerCard;

        public Announce FirstPlayerAnnounce => this.firstPlayerAnnounce;

        public Announce SecondPlayerAnnounce => this.secondPlayerAnnounce;

        public PlayerPosition GameClosedBy => this.whoClosedTheGame;


        public void Start()
        {
            IPlayer firstToPlay;
            IPlayer secondToPlay;
            IList<Card> firstToPlayCards;
            IList<Card> secondToPlayCards;

            if (this.whoWillPlayFirst == PlayerPosition.FirstPlayer)
            {
                firstToPlay = this.firstPlayer;
                firstToPlayCards = this.firstPlayerCards;
                secondToPlay = this.secondPlayer;
                secondToPlayCards = this.secondPlayerCards;
            }
            else
            {
                firstToPlay = this.secondPlayer;
                firstToPlayCards = this.secondPlayerCards;
                secondToPlay = this.firstPlayer;
                secondToPlayCards = this.firstPlayerCards;
            }

            var context = new PlayerTurnContext(this.state, deck.GetTrumpCard, deck.CardsLeft);

            PlayerAction firstPlayerAction = null;
            do
            {
                firstPlayerAction = this.FirstPlayerTurn(firstToPlay, context);

                if (!this.actionValidator.IsValid(firstPlayerAction, context,firstToPlayCards))
                {
                    // TODO: Do something more graceful.
                    throw new InternalGameException("Invalid turn!");
                }
            } while (firstPlayerAction.Type != PlayerActionType.PlayCard);

            context.FirstPlayedCard = firstPlayerAction.Card;

            PlayerAction secondPlayerAction =
                secondToPlay.GetTurn(context, this.actionValidator);

            if (!this.actionValidator.IsValid(secondPlayerAction, context, secondToPlayCards))
            {
                // TODO: Do something more graceful.
                throw new InternalGameException("Invalid turn!");
            }

            context.SecondPlayedCard = secondPlayerAction.Card;

            if (firstToPlay == this.firstPlayer)
            {
                this.firstPlayerCard = firstPlayerAction.Card;
                this.firstPlayerAnnounce = firstPlayerAction.Announce;
                this.secondPlayerCard = secondPlayerAction.Card;
                this.secondPlayerAnnounce = secondPlayerAction.Announce;
            }
            else
            {
                this.firstPlayerCard = secondPlayerAction.Card;
                this.firstPlayerAnnounce = secondPlayerAction.Announce;
                this.secondPlayerCard = firstPlayerAction.Card;
                this.secondPlayerAnnounce = firstPlayerAction.Announce;
            }

            firstToPlay.EndTurn(context);
            secondToPlay.EndTurn(context);

            ICardWinner cardWinner = new CardWinner();


            if (firstToPlay == this.firstPlayer)
            {
                this.winner = cardWinner.Winner(
                    firstPlayerAction.Card,
                    secondPlayerAction.Card,
                    this.deck.GetTrumpCard.Suit);
            }
            else
            {
                this.winner = cardWinner.Winner(
                    secondPlayerAction.Card,
                    firstPlayerAction.Card,
                    this.deck.GetTrumpCard.Suit);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        /// <returns>True => played card; False => another action</returns>
        private PlayerAction FirstPlayerTurn(
            IPlayer player,
            PlayerTurnContext context)
        {
            var playerTurn = player.GetTurn(context, this.actionValidator);

            if (playerTurn.Type == PlayerActionType.CloseGame)
            {
                this.state.Close();
                context.State = new FinalRoundState();
                this.state = new FinalRoundState();
                if (player == this.firstPlayer)
                {
                    this.whoClosedTheGame = PlayerPosition.FirstPlayer;
                }
                else
                {
                    this.whoClosedTheGame = PlayerPosition.SecondPlayer;
                }
            }

            if (playerTurn.Type == PlayerActionType.ChangeTrump)
            {
                var changeTrump = new Card(this.deck.GetTrumpCard.Suit, CardType.Nine);
                var oldTrump = this.deck.GetTrumpCard;
                context.TrumpCard = changeTrump;
                this.deck.ChangeTrumpCard(changeTrump);

                if (player == this.firstPlayer)
                {
                    this.firstPlayerCards.Remove(changeTrump);
                    this.firstPlayerCards.Add(oldTrump);
                    this.firstPlayer.AddCard(oldTrump);
                }
                else
                {
                    this.secondPlayerCards.Remove(changeTrump);
                    this.secondPlayerCards.Add(oldTrump);
                    this.secondPlayer.AddCard(oldTrump);
                }
            }

            return playerTurn;
        }


    }
}