using System.Collections.Generic;
using UnityEngine;

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

        public static readonly Dictionary<Player, Color> PlayerColorTank = new Dictionary<Player, Color>
        {
            { Player.X, new Color32(187,255,18,255) },
            { Player.O, new Color32(254,27,67,255) }
        };
        
        public static readonly Dictionary<ColorPalate, Color> ColorTank = new Dictionary<ColorPalate, Color>
        {
            { ColorPalate.LightYellow, new Color32(255,217,102,255) },
            { ColorPalate.DarkGreen, new Color32(16,125,111,255) }
        };
    }

    public enum ColorPalate
    {
        DarkGreen,
        LightYellow,
    }
}