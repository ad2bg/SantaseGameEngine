namespace Santase.Logic.RoundStates
{
    using System;

    public class FinalRoundState : BaseRoundState
    {
        public FinalRoundState(IGameRound round) : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => true;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => true;

        public override bool ShouldDrawCard => false;

        public override void PlayHand(int cardsLeftInDeck)
        {
        }
    }
}