namespace Santase.Logic
{
    public interface IGameRound
    {
        void Start();

        int TotalPointsWonByFirstPlayer { get; }

        int TotalPointsWonBySecondPlayer { get; }

    }
}