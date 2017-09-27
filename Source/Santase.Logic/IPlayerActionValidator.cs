namespace Santase.Logic
{
    using Santase.Logic.Players;

    public interface IPlayerActionValidator
    {
        bool IsValid(PlayerAction action, PlayerTurnContext context);
    }
}
