﻿namespace Santase.Logic
{
    using Santase.Logic.Players;

    public class SantaseGame : ISantaseGame
    {
        int firstPlayerTotalPoints;
        int secondPlayerTotalPoints;
        int roundsCount;

        private IPlayer firstPlayer;
        private IPlayer secondPlayer;

        private PlayerPosition firstToPlay;

        public SantaseGame(IPlayer firstPlayer, IPlayer secondPlayer, PlayerPosition firstToPlay)
        {
            this.firstPlayerTotalPoints = 0;
            this.secondPlayerTotalPoints = 0;
            this.roundsCount = 0;
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
            this.firstToPlay = firstToPlay;
        }

        public int FirstPlayerTotalPoints => this.firstPlayerTotalPoints;
        public int SecondPlayerTotalPoints => this.secondPlayerTotalPoints;

        public int RoundsPlayed => this.roundsCount;

        public void Start()
        {
            while (!this.IsGameOver())
            {
                this.PlayRound();
                this.roundsCount++;
            }
        }

        private void PlayRound()
        {
            IGameRound round = new GameRound(
                this.firstPlayer, 
                this.secondPlayer, 
                this.firstToPlay);

            round.Start();

            this.UpdatePoints(round);
        }

        private void UpdatePoints(IGameRound round)
        {
            if (round.ClosedByPlayer == PlayerPosition.FirstPlayer)
            {
                if (round.FirstPlayerPoints < 66)
                {
                    this.secondPlayerTotalPoints += 3;
                    this.firstToPlay = PlayerPosition.FirstPlayer; // Note: the next round will be started with the losing player, i.e. FirstPlayer
                    return;
                }
            }

            if (round.ClosedByPlayer == PlayerPosition.SecondPlayer)
            {
                if (round.SecondPlayerPoints < 66)
                {
                    this.firstPlayerTotalPoints += 3;
                    this.firstToPlay = PlayerPosition.SecondPlayer; // Note: the next round will be started with the losing player, i.e. SecondPlayer
                    return;
                }
            }


            if (round.FirstPlayerPoints<66 &&
                round.SecondPlayerPoints<66)
            {
                var winner = round.LastHandInPlayer;
                if (winner == PlayerPosition.FirstPlayer)
                {
                    this.firstPlayerTotalPoints += 1;
                    this.firstToPlay = PlayerPosition.SecondPlayer;
                    return;
                }
                else
                {
                    this.secondPlayerTotalPoints += 1;
                    this.firstToPlay = PlayerPosition.FirstPlayer;
                    return;
                }
            }


            if (round.FirstPlayerPoints > round.SecondPlayerPoints)
            {
                if (round.SecondPlayerPoints >= 33)
                {
                    this.firstPlayerTotalPoints += 1;

                }
                else if (round.SecondPlayerHasHand)
                {
                    this.firstPlayerTotalPoints += 2;
                }
                else
                {
                    this.firstPlayerTotalPoints += 3;
                }
                this.firstToPlay = PlayerPosition.SecondPlayer; // Note: the next round will be started with the losing player, i.e. SecondPlayer
            }
            else if (round.SecondPlayerPoints > round.FirstPlayerPoints)
            {
                if (round.FirstPlayerPoints >= 33)
                {
                    this.secondPlayerTotalPoints += 1;
                }
                else if (round.FirstPlayerHasHand)
                {
                    this.secondPlayerTotalPoints += 2;
                }
                else
                {
                    this.secondPlayerTotalPoints += 3;
                }
                this.firstToPlay = PlayerPosition.FirstPlayer; // Note: the next round will be started with the losing player, i.e. FirstPlayer
            }
            else
            {
                // Equal points from the round
            }
        }

        private bool IsGameOver()
        {
            return
                this.firstPlayerTotalPoints >= 11 ||
                this.secondPlayerTotalPoints >= 11;
        }
    }
}
