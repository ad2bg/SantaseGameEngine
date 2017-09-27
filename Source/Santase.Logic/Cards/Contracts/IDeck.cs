namespace Santase.Logic.Cards.Contracts
{
    public interface IDeck
    {
        Card GetNextCard();

        Card GetTrumpCard { get; }

        void ChangeTrumpCard(Card newCard);

        int CardsLeft { get; }
    }
}
