using System.Collections.Generic;
using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class Plot : MonoBehaviour
    {
        [SerializeField] private PointChannel selectPointChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private VoidChannel resetChannel;

        [SerializeField] private int row;
        [SerializeField] private int col;
        [SerializeField] private IntStorage askUnitIndex;
        [SerializeField] private UnitVisualize unitVisualize;
        [SerializeField] private Color playerXColor;
        [SerializeField] private Color playerOColor;

        private Dictionary<Player, Color> colorTank; // TODO: use a unifying game configuration

        private int currentStrength;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(VisualizeState);
            resetChannel.AddListener(ResetPlot);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(VisualizeState);
            resetChannel.RemoveListener(ResetPlot);
        }

        private void Start()
        {
            colorTank = new Dictionary<Player, Color>
            {
                { Player.X, playerXColor },
                { Player.O, playerOColor }
            };
        }

        private void OnMouseUpAsButton()
        {
            if (askUnitIndex.GetValue() < 0)
            {
                Debug.Log("Need to select a unit");
            }
            else if (askUnitIndex.GetValue() <= currentStrength)
            {
                Debug.Log("Invalid move with strength lower than current strength");
            }
            else
            {
                currentStrength = askUnitIndex.GetValue();
                selectPointChannel.ExecuteChannel(new Point(row,col,currentStrength));
            }
        }

        private void VisualizeState()
        {
            var currentState = gameStateStorage.GetValue();
            foreach (var plot in currentState.Board.GetGrid())
            {
                var checkPoint = plot.Key;
                if (checkPoint.Row == row && checkPoint.Col == col)
                {
                    unitVisualize.Visualize(checkPoint.Strength, colorTank[plot.Value]);
                }
            }
        }

        private void ResetPlot()
        {
            currentStrength = 0;
            unitVisualize.Disable();
        }
    }
}