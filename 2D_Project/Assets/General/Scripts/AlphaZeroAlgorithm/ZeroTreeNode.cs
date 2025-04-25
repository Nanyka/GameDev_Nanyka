using System.Collections.Generic;
using UnityEngine;

namespace AlphaZeroAlgorithm
{
    public class ZeroTreeNode
    {
        public GameState State { get; } // The game state this node represents
        public float Value { get; } // Value prediction for this state from the neural network
        public ZeroTreeNode Parent { get; } // The parent node
        public Move? LastMove { get; } // The move that led from the parent to this node

        public int TotalVisitCount { get; private set; } // Total visits to this node

        // Dictionary mapping legal moves from this state to their Branch statistics
        // Move must correctly implement Equals and GetHashCode for dictionary keys
        public Dictionary<Move, Branch> Branches { get; }

        // Dictionary mapping moves that have been explored and expanded to their child nodes
        public Dictionary<Move, ZeroTreeNode> Children { get; }


        /// <summary>
        /// Initializes a new MCTS tree node.
        /// </summary>
        /// <param name="state">The game state represented by this node.</param>
        /// <param name="value">The value prediction for this state from the neural network.</param>
        /// <param name="priors">A dictionary mapping all possible moves (decoded from model output) to their prior probabilities.</param>
        /// <param name="parent">The parent node (null for the root node).</param>
        /// <param name="lastMove">The move that led from the parent to this node (null for the root node).</param>
        public ZeroTreeNode(GameState state, float value, Dictionary<Move, float> priors, ZeroTreeNode parent, Move? lastMove)
        {
            State = state;
            Value = value;
            Parent = parent;
            LastMove = lastMove; // Nullable Move
            TotalVisitCount = 1; // Node is visited once upon creation/expansion

            Branches = new Dictionary<Move, Branch>();
            Children = new Dictionary<Move, ZeroTreeNode>();

            // Populate branches only for valid moves from the current state
            List<Move> legalMoves = state.LegalMoves(); // Get legal moves using GameState method

            foreach (var legalMove in legalMoves)
            {
                if (priors.TryGetValue(legalMove, out float priorProbability))
                {
                    Branches[legalMove] = new Branch(priorProbability);
                }
                else
                {
                    Debug.LogWarning($"ZeroTreeNode: No prior probability found for legal move {legalMove}. Using prior 0.");
                    Branches[legalMove] = new Branch(0f); // Use 0 prior as fallback
                }
            }
        }

        /// <summary>
        /// Gets the legal moves available from this node's state that have branches in the tree.
        /// </summary>
        /// <returns>An enumerable collection of legal moves.</returns>
        public IEnumerable<Move> GetMoves()
        {
            return Branches.Keys; // Return the keys (Moves) from the branches dictionary
        }

        /// <summary>
        /// Adds a child node to this node's children dictionary.
        /// </summary>
        /// <param name="move">The move that leads to the child node.</param>
        /// <param name="childNode">The child node.</param>
        public void AddChild(Move move, ZeroTreeNode childNode)
        {
            Children[move] = childNode; // Add the child node to the dictionary
        }

        /// <summary>
        /// Checks if this node has a child node for a given move.
        /// </summary>
        /// <param name="move">The move to check for a child node.</param>
        /// <returns>True if a child exists for the move, false otherwise.</returns>
        public bool HasChild(Move move)
        {
            return Children.ContainsKey(move); // Check if the move exists as a key in the children dictionary
        }

        /// <summary>
        /// Gets the child node for a given move.
        /// </summary>
        /// <param name="move">The move to get the child node for.</param>
        /// <returns>The child ZeroTreeNode.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no child exists for the move.</exception>
        public ZeroTreeNode GetChild(Move move)
        {
            return Children[move]; // Get the child node from the dictionary
        }

        /// <summary>
        /// Checks if this node has any children nodes.
        /// </summary>
        /// <returns>True if the children dictionary is empty, false otherwise.</returns>
        public bool IsNoChild()
        {
            return Children.Count == 0; // Check if the children dictionary is empty
        }

        /// <summary>
        /// Records a visit to this node through a specific branch.
        /// Updates visit counts and total value for the branch and the node.
        /// </summary>
        /// <param name="move">The move (branch) that was followed to reach this node.</param>
        /// <param name="value">The value to backpropagate (from the perspective of the player *whose turn it was at this node*).</param>
        public void RecordVisit(Move move, float value)
        {
            TotalVisitCount++;
            if (Branches.TryGetValue(move, out Branch branch))
            {
                branch.VisitCount++;
                branch.TotalValue += value;
                
                // if (move.Point.Row == 1 && move.Point.Col == 3 && move.Point.Strength == 3)
                //     Debug.Log($"Update value of {move}: {value}/{branch.TotalValue} , " +
                //               $"visit count {branch.VisitCount}" +
                //               $"--> expected value: {branch.TotalValue/branch.VisitCount}");
            }
            else
            {
                Debug.LogWarning($"ZeroTreeNode: Tried to record visit for move {move} which is not a branch from this node.");
            }

            // The value backpropagation itself happens iteratively up the parent chain in select_move.
        }


        /// <summary>
        /// Calculates the UCT score for a given move (branch) from this node.
        /// </summary>
        /// <param name="move">The move (branch).</param>
        /// <param name="uctC">The UCT exploration constant.</param>
        /// <returns>The UCT score.</returns>
        public float GetUctScore(Move move, float uctC)
        {
            if (!Branches.TryGetValue(move, out Branch branch))
            {
                // This should not happen if selecting from Branches.Keys, but as a safeguard
                Debug.LogError($"ZeroTreeNode: Calculating UCT for non-existent branch: {move}");
                return float.NegativeInfinity; // Return lowest possible score
            }
            
            float q = branch.VisitCount == 0 ? 0.0f : branch.TotalValue / branch.VisitCount; // Expected value (Q)
            float p = branch.Prior; // Prior probability
            int n = branch.VisitCount; // Visit count for this branch
            int totalN = TotalVisitCount; // Total visits to this node (parent)
            float explorationTerm = uctC * p * Mathf.Sqrt(totalN) / (n + 1);

            return q + explorationTerm;
        }


        /// <summary>
        /// Gets the expected value (average value) for a move's branch.
        /// </summary>
        /// <param name="move">The move (branch).</param>
        /// <returns>The expected value.</returns>
        public float GetExpectedValue(Move move)
        {
            if (!Branches.TryGetValue(move, out Branch branch)) return 0.0f;
            if (branch.VisitCount == 0) return 0.0f;
            return branch.TotalValue / branch.VisitCount;
        }

        /// <summary>
        /// Gets the prior probability for a move's branch.
        /// </summary>
        /// <param name="move">The move (branch).</param>
        /// <returns>The prior probability.</returns>
        public float GetPrior(Move move)
        {
            if (Branches.TryGetValue(move, out Branch branch)) return branch.Prior;
            return 0.0f; // Should not happen for moves in Branches.Keys
        }

        /// <summary>
        /// Gets the visit count for a move's branch.
        /// </summary>
        /// <param name="move">The move (branch).</param>
        /// <returns>The visit count.</returns>
        public int GetVisitCount(Move move)
        {
            if (Branches.TryGetValue(move, out Branch branch)) return branch.VisitCount;
            return 0; // Should not happen for moves in Branches.Keys
        }
    }
}