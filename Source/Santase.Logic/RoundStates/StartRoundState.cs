namespace Santase.Logic.RoundStates
{
    using System;

    public class StartRoundState : BaseRoundState
    {
        public StartRoundState(IGameRound round) : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => false;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => false;

        public override bool ShouldDrawCard => true;

        public override void PlayHand(int cardsLeftInDeck)
        {
            this.round.SetState(new MoreThan2CardsLeftRoundState(this.round));
        }
    }
}
