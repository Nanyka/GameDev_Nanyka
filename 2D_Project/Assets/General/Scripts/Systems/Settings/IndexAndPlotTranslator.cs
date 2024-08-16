using UnityEngine;

namespace TheAiAlchemist
{
    [CreateAssetMenu(fileName = "IndexAndPositionTranslator", menuName = "TheAiAlchemist/Settings/IndexAndPositionTranslator")]

    public class IndexAndPlotTranslator : ScriptableObject
    {
        public int PlotToIndex(Vector3 plot)
        {
            return Mathf.RoundToInt(plot.x + plot.y * 3 + 4);
        }

        public Vector3 IndexToPlot(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector3(-1f, -1f, 0f);
                case 1:
                    return new Vector3(0f, -1f, 0f);
                case 2:
                    return new Vector3(1f, -1f, 0f);
                case 3:
                    return new Vector3(-1f, 0f, 0f);
                case 4:
                    return new Vector3(0f, 0f, 0f);
                case 5:
                    return new Vector3(1f, 0f, 0f);
                case 6:
                    return new Vector3(-1f, 1f, 0f);
                case 7:
                    return new Vector3(0f, 1f, 0f);
                case 8:
                    return new Vector3(1f, 1f, 0f);
            }
            return Vector3.negativeInfinity;
        }
        
        public (int, int, int)[] winningCombinations = {
            (0, 1, 2), (3, 4, 5), (6, 7, 8), // Horizontal
            (0, 3, 6), (1, 4, 7), (2, 5, 8), // Vertical
            (0, 4, 8), (2, 4, 6) // Diagonal
        };
    }
}