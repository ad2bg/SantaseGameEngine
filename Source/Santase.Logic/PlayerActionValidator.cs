
namespace Santase.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using Santase.Logic.RoundStates;

    public class PlayerActionValidator : IPlayerActionValidator
    {
        public bool IsValid(PlayerAction action, PlayerTurnContext context, IList<Card> playerCards)
        {
            if (!context.AmItheFirstPlayer)
            {
                action.Announce = Announce.None;
            }

            if (action.Type == PlayerActionType.PlayCard)
            {
                if (!playerCards.Contains(action.Card))
                {
                    return false;
                }


                if (action.Announce != Announce.None)
                {
                    if (action.Card.Type != CardType.King && action.Card.Type != CardType.Queen)
                    {
                        action.Announce = Announce.None;
                    }
                    // TODO: Check announce for another card
                }

                if (context.State.ShouldObserveRules)
                {
                    if (!context.AmItheFirstPlayer)
                    {
                        var firstCard = context.FirstPlayedCard;
                        var ourCard = action.Card;

                        if (firstCard.Suit != ourCard.Suit)
                        {
                            if (ourCard.Suit != context.TrumpCard.Suit)
                            {
                                var hasTrump = playerCards.Any(c => c.Suit == context.TrumpCard.Suit);
                                if (hasTrump)
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            if (ourCard.GetValue() < firstCard.GetValue())
                            {
                                var hasBigger = playerCards.Any(c => c.GetValue() > firstCard.GetValue());
                                if (hasBigger)
                                {
                                    return false;
                                }
                            }
                        }

                    }
                }
            }

            if (action.Type == PlayerActionType.CloseGame && !(context.State is FinalRoundState))
            {
                if (!context.State.CanClose || !context.AmItheFirstPlayer)
                {
                    return false;
                }
            }

            if (action.Type == PlayerActionType.ChangeTrump)
            {
                if (!context.State.CanChangeTrump || !context.AmItheFirstPlayer)
                {
                    return false;
                }

                if (!playerCards.Contains(new Card(context.TrumpCard.Suit, CardType.Nine)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
