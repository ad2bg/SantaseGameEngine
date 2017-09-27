using Santase.Logic.Cards;

namespace Santase.Logic
{
    public class GameHand : IGameHand
    {
        public GameHand()
        {

        }

        public PlayerPosition Winner => throw new System.NotImplementedException();

        public Card FirstPlayerCard => throw new System.NotImplementedException();

        public Card SecondPlayerCard => throw new System.NotImplementedException();

        public Announce FirstPlayerAnnounce => throw new System.NotImplementedException();

        public Announce SecondPlayerAnnounce => throw new System.NotImplementedException();

        public void Start()
        {
            throw new System.NotImplementedException();
        }
    }
}