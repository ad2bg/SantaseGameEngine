namespace Santase.Logic
{
    internal interface IGameHand
    {
        PlayerPosition Winner { get; }

        void Start();
    }
}