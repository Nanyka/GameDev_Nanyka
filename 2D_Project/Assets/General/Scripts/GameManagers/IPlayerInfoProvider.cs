namespace TheAiAlchemist
{
    public interface IPlayerInfoProvider
    {
        public IPlayerBehaviorStorage GetPlayerInfo(int playerId);
        public IPlayerBehaviorStorage GetOpponentInfo(int playerId);
    }
}