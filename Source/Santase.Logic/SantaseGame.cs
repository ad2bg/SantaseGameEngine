using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Santase.Logic
{
    public class SantaseGame : ISantaseGame
    {
        int firstPlayerTotalPoints;
        int secondPlayerTotalPoints;

        public SantaseGame()
        {
            this.firstPlayerTotalPoints = 0;
            this.secondPlayerTotalPoints = 0;
        }

        public int FirstPlayerTotalPoints => this.firstPlayerTotalPoints;
        public int SecondPlayerTotalPoints => this.secondPlayerTotalPoints;

        public void Start()
        {
            while (!this.IsGameOver())
            {
                this.PlayRound();
            }
        }

        private void PlayRound()
        {
            IGameRound round = new GameRound();
            round.Start();
            this.firstPlayerTotalPoints += round.TotalPointsWonByFirstPlayer;
            this.secondPlayerTotalPoints += round.TotalPointsWonBySecondPlayer;
        }

        private bool IsGameOver()
        {
            return
                this.firstPlayerTotalPoints >= 11 ||
                this.secondPlayerTotalPoints >= 11;
        }
    }
}
