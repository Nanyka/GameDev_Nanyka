namespace AlphaZeroAlgorithm
{
    public static class GameConstants
    {
        public const int BoardSize = 3;
        public const int NumStrengths = 3;

        // 1-based row/column ranges
        public static readonly int[] Rows = { 1, 2, 3 };
        public static readonly int[] Cols = { 1, 2, 3 };

        // Define diagonals using the C# Point struct (1-based)
        public static readonly Point[] Diag1 = { new Point(1, 1, 1), new Point(2, 2, 1), 
            new Point(3, 3, 1) };
        public static readonly Point[] Diag2 = { new Point(1, 3, 1), new Point(2, 2, 1), 
            new Point(3, 1, 1) };

        // Note: The strength (1) in the diagonal Point definitions in Python's DIAG_1/DIAG_2
        // might just be a placeholder, as winning only depends on having *any* piece
        // of the same player in a line, not a specific strength. Our C# Player class
        // stores just the player, so the strength in the Point used as a key in the Board grid
        // will determine which piece is there. The win condition check (_has_3_in_a_row)
        // should only check the player, ignoring strength.
    }
}