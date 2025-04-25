namespace AlphaZeroAlgorithm
{
    public enum Player
    {
        X = 1,
        O = 2
    }

    public static class PlayerExtensions
    {
        public static Player Other(this Player player)
        {
            return player == Player.O ? Player.X : Player.O;
        }
    }
    
    // You might put this in a 'Agents' or 'Core' namespace
    // using YourGameNamespace.Core;

    // Interface for agents (human or AI)
}