using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Sentis; 
using UnityEngine; 
using System.Threading.Tasks; 

namespace AlphaZeroAlgorithm
{
    public class AlphaZeroAgent : IAgent
    {
        private Model _model;
        private IWorker _worker; 
        private Encoder _encoder; 
        private int _numRounds; 
        private float _c;
        private bool _printOut;

        // Constructor
        /// <summary>
        /// Initializes the TTTNZeroAgent_v2.
        /// </summary>
        /// <param name="model">The ONNX model used for this agent</param>
        /// <param name="worker">The Sentis IWorker configured with the ONNX model.</param>
        /// <param name="encoder">The TTTRelativeEncoder instance.</param>
        /// <param name="roundsPerMove">Number of MCTS simulations to perform for each move.</param>
        /// <param name="c">The UCT exploration constant.</param>
        /// <param name="printOut">Whether to print debug information during MCTS.</param>
        public AlphaZeroAgent(ModelAsset model, int roundsPerMove = 8, float c = 2.0f,
            bool printOut = false)
        {
            if (roundsPerMove <= 0)
                throw new ArgumentOutOfRangeException(nameof(roundsPerMove), "Rounds per move must be positive.");
            if (c <= 0) throw new ArgumentOutOfRangeException(nameof(c), "UCT constant must be positive.");

            _encoder = new Encoder();
            _model = ModelLoader.Load(model);
            _worker = WorkerFactory.CreateWorker(BackendType.CPU, _model);
            _numRounds = roundsPerMove;
            _c = c;
            _printOut = printOut;
        }

        /// <summary>
        /// Selects the best move for the current game state using MCTS.
        /// This method runs the MCTS simulation rounds.
        /// </summary>
        /// <param name="gameState">The current GameState.</param>
        /// <returns>The chosen Move.</returns>
        public async Task<Move> SelectMove(GameState gameState)
        {
            if (gameState.IsOver())
            {
                Debug.LogWarning($"Agent asked to select move for a game that is already over. " +
                                 $"Player: {gameState.NextPlayer}");
                return Move.Resign;
            }

            if (_printOut)
                Debug.Log($"Agent ({gameState.NextPlayer}) thinking... Running MCTS for {_numRounds} rounds.");

            // 1. Create the root node for the current game state.
            ZeroTreeNode root = CreateNode(gameState);

            // 2. Run MCTS simulation rounds
            for (int i = 0; i < _numRounds; i++)
            {
                ZeroTreeNode node = root;
                Move selectedMove = SelectBranch(node);

                // List<(ZeroTreeNode node, Move move)> path = new List<(ZeroTreeNode node, Move move)>();
                // path.Add((node, selectedMove));

                while (node.HasChild(selectedMove)) // While we can move down the tree (branch is expanded)
                {
                    node = node.GetChild(selectedMove);
                    if (node.State.IsOver()) break;
                    selectedMove = SelectBranch(node);
                    // path.Add((node, selectedMove)); // Add this step to the path
                }

                GameState currentState = node.State;
                float value;
                GameState nextState = currentState.ApplyMove(selectedMove);

                if (nextState.IsOver())
                {
                    var winner = nextState.Winner();
                    if (winner.HasValue && winner.Value == gameState.NextPlayer)
                        value = 1.0f;
                    else if (winner.HasValue && winner.Value != gameState.NextPlayer)
                        value = -1.0f;
                    else
                        value = 0.0f;
                }
                else
                {
                    ZeroTreeNode childNode = CreateNode(nextState, move: selectedMove, parent: node);
                    value = -1.0f * childNode.Value;
                }

                BackPropagate(node, selectedMove, value);
            } // End of MCTS simulation rounds

            var visitedBranches = root.Branches.Where(pair => pair.Value.VisitCount > 0).ToList();

            if (!visitedBranches.Any())
            {
                Debug.LogWarning("No branches visited in MCTS. Falling back to selecting a random legal move.");
                List<Move> legalMovesFallback = gameState.LegalMoves();
                if (legalMovesFallback.Any())
                {
                    System.Random random = new System.Random();
                    return legalMovesFallback[random.Next(legalMovesFallback.Count)];
                }
                else
                {
                    Debug.LogWarning("No legal moves available. Resigning.");
                    return Move.Resign; // If no legal moves, resign
                }
            }

            var bestBranch = visitedBranches.Aggregate(
                (b1, b2) =>
                    b1.Value.VisitCount > b2.Value.VisitCount ? b1 : b2);
            Move finalMove = bestBranch.Key; // The move with the highest visit count

            if (_printOut)
                Debug.Log($"Agent ({gameState.NextPlayer}) selected move: {finalMove} " +
                          $"(Visits: {bestBranch.Value.VisitCount})");

            return finalMove;
        }

        public Move GetMoveFromInput(string inputString)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Selects a branch (move) from a node based on the UCT score.
        /// </summary>
        /// <param name="node">The current node.</param>
        /// <returns>The selected Move.</returns>
        private Move SelectBranch(ZeroTreeNode node)
        {
            var bestBranch = node.Branches.Aggregate(
                (b1, b2) =>
                    node.GetUctScore(b1.Key, _c) > node.GetUctScore(b2.Key, _c) ? b1 : b2);

            return bestBranch.Key; // Return the move with the highest UCT score
        }

        private void BackPropagate(ZeroTreeNode node, Move move, float value)
        {
            while (node != null)
            {
                node.RecordVisit(move, value);
                move = node.LastMove;
                node = node.Parent;
                value = -1 * value;
            }
        }

        private ZeroTreeNode CreateNode(GameState gameState, Move move = null, ZeroTreeNode parent = null)
        {
            if (gameState.IsOver())
            {
                float terminalValue = 0.0f;
                var winner = gameState.Winner();
                if (!winner.HasValue)
                    return new ZeroTreeNode(gameState, terminalValue, new Dictionary<Move, float>(), parent, move);

                if (winner.Value == gameState.NextPlayer)
                {
                    terminalValue = 1.0f;
                }
                else if (winner.Value != gameState.NextPlayer)
                {
                    terminalValue = -1.0f; 
                }
                else
                {
                    terminalValue = 0.0f;
                }
                return new ZeroTreeNode(gameState, terminalValue, new Dictionary<Move, float>(), parent, move);
            }
            
            string input1Name = _model.inputs[0].name; 
            string input2Name = _model.inputs[1].name;
            var (stateTensor, invTensor) = _encoder.Encode(gameState);

            TensorFloat modelInputBoard = stateTensor; 
            TensorFloat modelInputInv = invTensor; 

            var inputs = new Dictionary<string, Tensor>()
            {
                { input1Name, modelInputBoard },
                { input2Name, modelInputInv }
            };

            _worker.Execute(inputs); 
            
            string output2Name = _model.outputs[1];
            TensorFloat policyOutput =_worker.PeekOutput() as TensorFloat; 
            TensorFloat valueOutput = _worker.PeekOutput(output2Name) as TensorFloat;

            float[] priorsArray = policyOutput.ToReadOnlyArray();
            valueOutput.MakeReadable();
            float valuePrediction = valueOutput[0];
            
            Dictionary<Move, float> movePriors = new Dictionary<Move, float>();
            for (int idx = 0; idx < priorsArray.Length; idx++) 
            {
                Move decodedMove = _encoder.DecodeMoveIndex(idx); 
                movePriors[decodedMove] = priorsArray[idx];
            }
            
            modelInputBoard.Dispose();
            modelInputInv.Dispose();
            policyOutput.Dispose();
            valueOutput.Dispose();
            
            ZeroTreeNode newNode = new ZeroTreeNode(gameState, valuePrediction, movePriors, parent, move);

            if (parent != null)
                parent.AddChild(move, newNode); 

            return newNode;
        }
    }
}