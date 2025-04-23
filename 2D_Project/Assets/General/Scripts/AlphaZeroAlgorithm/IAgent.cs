namespace AlphaZeroAlgorithm
{
    public interface IAgent
    {
        // This method should return the agent's chosen move for the current game state.
        // In console applications, this might involve blocking for input.
        // In Unity, this typically triggers a UI workflow or AI calculation,
        // and the actual move is returned via a callback or another mechanism.
        Move SelectMove(GameState gameState);
        public Move GetMoveFromInput(string inputString);
    }
}