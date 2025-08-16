using System;
using System.Collections;
using System.Collections.Generic;
using AlphaZeroAlgorithm;
using UnityEngine;

namespace TheAiAlchemist
{
    public class Plot : MonoBehaviour
    {
        [SerializeField] protected PointChannel selectPointChannel;
        [SerializeField] private VoidChannel changePlayerChannel;
        [SerializeField] private GameStateStorage gameStateStorage;
        [SerializeField] private VoidChannel resetChannel;
        [SerializeField] private BoolChannel endGameChannel;

        [SerializeField] protected int row;
        [SerializeField] protected int col;
        [SerializeField] protected IntStorage askUnitIndex;
        [SerializeField] private UnitVisualize unitVisualize;

        protected int currentStrength;
        private int visualStrength;

        private void OnEnable()
        {
            changePlayerChannel.AddListener(VisualizeState);
            resetChannel.AddListener(ResetPlot);
            endGameChannel.AddListener(HighlightWinner);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(VisualizeState);
            resetChannel.RemoveListener(ResetPlot);
            endGameChannel.RemoveListener(HighlightWinner);
        }

        // private void Start()
        // {
        //     // ResetPlot();
        //     VisualizeState();
        // }

        protected virtual void OnMouseUpAsButton()
        {
            // Debug.Log($"Mouse Up at {row}, {col}");
            if (askUnitIndex.GetValue() < 0)
            {
                // Debug.Log("Need to select a unit");
            }
            else if (askUnitIndex.GetValue() <= currentStrength)
            {
                // Debug.Log("Invalid move with strength lower than current strength");
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
            if (currentState == null) return;
            
            foreach (var plot in currentState.Board.GetGrid())
            {
                var checkPoint = plot.Key;
                if (checkPoint.Row == row && checkPoint.Col == col)
                {
                    if (checkPoint.Strength <= visualStrength) continue;
                    bool isBeatOpponent = checkPoint.Strength > visualStrength && visualStrength != 0;
                    unitVisualize.Visualize(checkPoint.Strength, plot.Value, isBeatOpponent);
                    visualStrength = checkPoint.Strength;
                }
            }
        }

        private void ResetPlot()
        {
            // Debug.Log("Reset plot");
            currentStrength = 0;
            visualStrength = 0;
            unitVisualize.Disable();
        }
        
        private void HighlightWinner(bool hasWinner)
        {
            if (hasWinner)
            {
                var winnerPoints = gameStateStorage.GetValue().GetWinningLine();
                if (winnerPoints != null)
                    foreach (Point p in winnerPoints)
                    {
                        if (p.Row == row && p.Col == col)
                        {
                            // Debug.Log("Match found at Point (" + p.Row + ", " + p.Col + ")");
                            // unitVisualize.Highlight();
                            StartCoroutine(WaitToHighlight());
                            return;
                        }
                    }
            }
        }
        
        private IEnumerator WaitToHighlight()
        {
            yield return new WaitForSeconds(0.5f);
            unitVisualize.Highlight();
        }
    }
}