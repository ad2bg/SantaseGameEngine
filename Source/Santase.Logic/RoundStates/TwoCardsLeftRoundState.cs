﻿namespace Santase.Logic.RoundStates
{
    using System;

    class TwoCardsLeftRoundState : BaseRoundState
    {
        public TwoCardsLeftRoundState(IGameRound round) : base(round)
        {
        }

        public override bool CanAnnounce20Or40 => true;

        public override bool CanClose => false;

        public override bool CanChangeTrump => false;

        public override bool ShouldObserveRules => false;

        public override bool ShouldDrawCard => true;

        internal override void PlayHand(int cardsLeftInDeck)
        {
            this.round.SetState(new FinalRoundState(this.round));
        }
    }
}
