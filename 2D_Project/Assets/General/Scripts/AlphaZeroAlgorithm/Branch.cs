namespace AlphaZeroAlgorithm
{
    public class Branch
    {
        public float Prior { get; set; } // Prior probability from the neural network
        public int VisitCount { get; set; } // Number of times this branch has been visited
        public float TotalValue { get; set; } // Sum of values backpropagated through this branch

        public Branch(float prior)
        {
            Prior = prior;
            VisitCount = 0;
            TotalValue = 0.0f;
        }
    }
}