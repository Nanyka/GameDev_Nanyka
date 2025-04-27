using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Sentis; 
using UnityEngine; 
using System.Threading.Tasks; 

namespace AlphaZeroAlgorithm
{
    public class AlphaZeroAgent : IAgent
    {
        private Model _model;
        private Worker _worker; 
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
            _worker = new Worker(_model, BackendType.CPU);
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
            ZeroTreeNode root = await CreateNode(gameState);

            // 2. Run MCTS simulation rounds
            for (int i = 0; i < _numRounds; i++)
            {
                // Debug.Log($"----- ROUND ({i}) -----");

                ZeroTreeNode node = root;
                Move selectedMove = SelectBranch(node);


                int printPeriod = 0;
                while (node.HasChild(selectedMove)) // While we can move down the tree (branch is expanded)
                {
                    printPeriod++;
                    node = node.GetChild(selectedMove);


                    // if (printPeriod == 1)
                    //     PrintNode(node);
                    
                    
                    if (node.State.IsOver()) break;
                    // Debug.Log($"Travel {selectedMove}: branch length {node.Branches.Count}");
                    selectedMove = SelectBranch(node);
                }

                GameState currentState = node.State;
                float value;
                GameState nextState = currentState.ApplyMove(selectedMove);

                if (nextState.IsOver())
                {
                    var winner = nextState.Winner();
                    value = winner.HasValue? 1 : 0;
                }
                else
                {
                    ZeroTreeNode childNode = await CreateNode(nextState, move: selectedMove, parent: node);
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

                Debug.LogWarning("No legal moves available. Resigning.");
                return Move.Resign; // If no legal moves, resign
            }

            var bestBranch = visitedBranches.Aggregate(
                (b1, b2) =>
                    b1.Value.VisitCount > b2.Value.VisitCount ? b1 : b2);
            Move finalMove = bestBranch.Key; // The move with the highest visit count

            if (_printOut)
            {
                Debug.Log($"Agent ({gameState.NextPlayer}) selected move: {finalMove} " +
                          $"(Visits: {bestBranch.Value.VisitCount})");
                Debug.Log($"Total visit count: {root.TotalVisitCount}");
            }
            
            // PrintNode(root);

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

        private async Task<ZeroTreeNode> CreateNode(GameState gameState, Move move = null, ZeroTreeNode parent = null)
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
            
            var input1Name = _model.inputs[0].name; 
            var input2Name = _model.inputs[1].name;
            var (stateTensor, invTensor) = _encoder.Encode(gameState);

            _worker.SetInput(input1Name,stateTensor);
            _worker.SetInput(input2Name,invTensor);
            _worker.Schedule(); 
            
            var output2Name = _model.outputs[1].name;
            var policyOutput =_worker.PeekOutput() as Tensor<float>; 
            var valueOutput = _worker.PeekOutput(output2Name) as Tensor<float>;

            policyOutput = await policyOutput.ReadbackAndCloneAsync();
            valueOutput = await valueOutput.ReadbackAndCloneAsync();
            
            var priorsArray = policyOutput.DownloadToArray();
            var valuePrediction = valueOutput[0];
            
            Dictionary<Move, float> movePriors = new Dictionary<Move, float>();
            for (int idx = 0; idx < priorsArray.Length; idx++) 
            {
                var decodedMove = _encoder.DecodeMoveIndex(idx); 
                movePriors[decodedMove] = priorsArray[idx];
            }
            
            stateTensor.Dispose();
            invTensor.Dispose();
            policyOutput.Dispose();
            valueOutput.Dispose();
            
            ZeroTreeNode newNode = new ZeroTreeNode(gameState, valuePrediction, movePriors, parent, move);

            if (move != null) parent.AddChild(move, newNode);

            return newNode;
        }

        public void DisableAiElements()
        {
            _worker.Dispose();
        }
        
        private void PrintNode(ZeroTreeNode nodeToVisualize)
        {
            // Use a StringBuilder to build the complete output string
            StringBuilder outputStringBuilder = new StringBuilder();

            outputStringBuilder.AppendLine("--- MCTS Root Node Statistics (by Strength and Position) ---");

            // Iterate through each possible strength (1, 2, 3)
            for (int strength = 1; strength <= GameConstants.NumStrengths; strength++)
            {
                outputStringBuilder.AppendLine($"\nStr {strength}:"); // Append strength header
                outputStringBuilder.AppendLine("  A      B      C"); // Append column headers

                // Iterate through rows (1, 2, 3)
                for (int row = 1; row <= GameConstants.BoardSize; row++)
                {
                    string rowPrefix = $"{row} "; // Start the row output with the row number
                    List<string> cells = new List<string>(); // To hold the formatted strings for each column

                    // Iterate through columns (1, 2, 3)
                    for (int col = 1; col <= GameConstants.BoardSize; col++)
                    {
                        // Construct the Move object for this specific strength and position
                        Point possiblePoint = new Point(row, col, strength);
                        Move possibleMove = new Move(possiblePoint); // Move constructor takes Point

                        // Check if this move exists as a branch in the node's legal moves
                        // Using TryGetValue is efficient as it avoids throwing an exception if the key doesn't exist
                        if (nodeToVisualize.Branches.TryGetValue(possibleMove, out Branch branch))
                        {
                            // If the branch exists, get its statistics
                            float expectedValue = nodeToVisualize.GetExpectedValue(possibleMove);
                            int visitCount = nodeToVisualize.GetVisitCount(possibleMove);
                            float prior = nodeToVisualize.GetPrior(possibleMove); // Prior is in Branch directly
                            float uctScore = nodeToVisualize.GetUctScore(possibleMove, _c); // Calculate UCT if needed

                            // Format the cell string similar to (ExpectedValue), (VisitCount)
                            // Using F4 for 4 decimal places for float values
                            cells.Add($"({prior:F4}),({visitCount}),({expectedValue:F4}),({uctScore:F4})");
                        }
                        else
                        {
                            // If the move is not a legal move from this node (no branch), print zeros
                            cells.Add($"(0.0000),(00),(0.0000),(0.0000)");
                        }
                    }

                    // Join the formatted cell strings with a separator and append to the row output
                    rowPrefix += string.Join("|", cells);
                    outputStringBuilder.AppendLine(rowPrefix); // Append the completed row string
                }
            }

            outputStringBuilder.AppendLine("---------------------------------------------------");
            Debug.Log(outputStringBuilder.ToString());
        }
    }
}