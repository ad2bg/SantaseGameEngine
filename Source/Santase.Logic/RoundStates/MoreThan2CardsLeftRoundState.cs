namespace Santase.Logic.RoundStates
{
    using System;

    public class MoreThan2CardsLeftRoundState : BaseRoundState
    {
        public MoreThan2CardsLeftRoundState(IGameRound round) : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => true; // This is generally; Additional logic for each player determines whether he can announce (base on whether he's already won a hand)

        public override bool CanClose => true; // This is generally; The player needs to be firstToPlay a hand

        public override bool CanChangeTrump => true; // This is generally; The player needs to be firstToPlay a hand

        public override bool ShouldObserveRules => false;

        public override bool ShouldDrawCard => true;

        internal override void PlayHand(int cardsLeftInDeck)
        {
            if (cardsLeftInDeck == 2)
            {
                this.round.SetState(new TwoCardsLeftRoundState(this.round));
            }
        }
    }
}
