
namespace Santase.Logic
{
    using Santase.Logic.Cards;
    using Santase.Logic.Cards.Contracts;
    using Santase.Logic.Players;
    using Santase.Logic.RoundStates;

    public class GameHand : IGameHand
    {
        private PlayerPosition whoWillPlayFirst;
        private IPlayer firstPlayer;
        private IPlayer secondPlayer;
        private BaseRoundState state;
        private IDeck deck;
        private IPlayerActionValidator actionValidator;

        public GameHand(
            PlayerPosition whoWillPlayFirst,
            IPlayer firstPlayer,
            IPlayer secondPlayer,
            BaseRoundState state,
            IDeck deck)
        {
            this.whoWillPlayFirst = whoWillPlayFirst;
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.state = state;
            this.deck = deck;
            this.actionValidator = new PlayerActionValidator();
        }

        public PlayerPosition Winner => throw new System.NotImplementedException();

        public Card FirstPlayerCard => throw new System.NotImplementedException();

        public Card SecondPlayerCard => throw new System.NotImplementedException();

        public Announce FirstPlayerAnnounce => throw new System.NotImplementedException();

        public Announce SecondPlayerAnnounce => throw new System.NotImplementedException();

        public PlayerPosition GameClosedBy => throw new System.NotImplementedException();

        public void Start()
        {
            IPlayer firstToPlay;
            IPlayer secondToPlay;

            if (this.whoWillPlayFirst == PlayerPosition.FirstPlayer)
            {
                firstToPlay = this.firstPlayer;
                secondToPlay = this.secondPlayer;
            }
            else
            {
                firstToPlay = this.secondPlayer;
                secondToPlay = this.firstPlayer;
            }

            var context = new PlayerTurnContext(this.state, deck.GetTrumpCard, deck.CardsLeft);

            PlayerAction firstPlayerAction = null;
            do
            {
                firstPlayerAction = this.FirstPlayerTurn(firstToPlay, context);

                if (!this.actionValidator.IsValid(firstPlayerAction, context))
                {
                    // TODO: DO something more graceful.
                    throw new InternalGameException("Invalid turn!");
                }
            } while (firstPlayerAction.Type != PlayerActionType.PlayCard);

            context.FirstPlayedCard = firstPlayerAction.Card;

            PlayerAction secondPlayerAction = secondToPlay.GetTurn(
                new PlayerTurnContext(
                    this.state,
                    deck.GetTrumpCard,
                    deck.CardsLeft),
                this.actionValidator); // ??? context ???

            context.SecondPlayedCard = secondPlayerAction.Card;

            // TODO: if turn == close => close, change state, ask firstToPlay
            // TODO: if turn == trumpChange => change, ask firstToPlay


            // TODO: determine who wins the hand

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
                // TODO: who closed the game
            }

            if (playerTurn.Type == PlayerActionType.ChangeTrump)
            {
                // TODO: Change trump
            }

            if (playerTurn.Type == PlayerActionType.PlayCard)
            {
                // TODO: Card played               
            }

            return playerTurn;
        }


    }
}